namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

public sealed class DefaultRabbitMQPersistentConnection
       : IRabbitMQPersistentConnection, IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private readonly string _queueName;
    private readonly int _retryCount;
    private bool _disposed = false;

    private readonly object sync_root = new object();


    public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, string queueName, int retryCount = 5)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _queueName = queueName;
        _retryCount = retryCount;
    }

    public string GetQueue()
    {
        return _queueName;
    }

    public bool IsConnected
    {
        get
        {
            return _connection != null && _connection.IsOpen && !_disposed;
        }
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }
        return _connection.CreateModel();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
            _disposed = true;
        }
    }

    public bool TryConnect()
    {
        lock (sync_root)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    // TODO;
                }
            );

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;
        TryConnect();
    }

    void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;
        TryConnect();
    }

    void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (_disposed) return;
        TryConnect();
    }
}