namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Globalization;
using System.Text;


public abstract class RabbitMqBus : IMessageBus
{
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly string _queueName;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConnectionModel _connectionModel;
    private IModel _channel;

    public RabbitMqBus(IRabbitMQPersistentConnection persistentConnection
        , ConnectionModel connectionModel
        , IServiceProvider serviceProvider)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _queueName = _persistentConnection.GetQueue();
        _connectionModel = connectionModel;
        _serviceProvider = serviceProvider;
    }

    public async Task CompleteAsync(string messageToken)
    {
        ulong deliveryTag = ulong.Parse(messageToken, NumberStyles.Integer, CultureInfo.InvariantCulture);
        _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        await Task.Delay(0).ConfigureAwait(false);
    }

    public void Publish(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using (var channel = _persistentConnection.CreateModel())
        {
            DeclareQueue(channel);
            PublishMessage(channel, message);
        }
    }

    public void Publish(IEnumerable<Message> messages)
    {
        if (messages == null || !messages.Any())
        {
            throw new ArgumentNullException(nameof(messages));
        }

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using (var channel = _persistentConnection.CreateModel())
        {
            DeclareQueue(channel);
            foreach (var message in messages)
            {
                PublishMessage(channel, message);
            }
        }
    }

    public void RegisterHandler<T, TH>()
        where T : Message
        where TH : IIntegrationEventHandler<T>
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        _channel = _persistentConnection.CreateModel();

        DeclareQueue(_channel);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += ProcessMessagesAsync<T, TH>;

        if (_connectionModel.PrefetchCount > 1)
        {
            _channel.BasicQos(0, (ushort)_connectionModel.PrefetchCount, true);
        }

        _channel.BasicConsume(queue: _queueName
            , autoAck: false
            , consumer: consumer);
    }

    private void DeclareQueue(IModel channel)
    {
        var arguments = new Dictionary<string, object>();

        // Message Duplicate Detection: https://github.com/noxdafox/rabbitmq-message-deduplication
        if (_connectionModel.IsDuplicateDetectionEnabled)
        {
            // Enable message duplicate detection
            arguments["x-message-deduplication"] = true;

            // Store message IDs for 10 minutes to check for duplicates
            arguments["x-cache-ttl"] = 600000;
        }

        channel.QueueDeclare(queue: _queueName
            , durable: true
            , exclusive: false
            , autoDelete: false
            , arguments: arguments);
    }

    private void PublishMessage(IModel channel, Message message)
    {
        string serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2; // persistent
        properties.Headers = new Dictionary<string, object>
            {
                { "x-deduplication-header", message.Id }, // Messages with the same value in this header will be considered duplicated
            };

        channel.BasicPublish(exchange: ""
            , routingKey: _queueName
            , mandatory: true
            , basicProperties: properties
            , body: body);
    }

    private void ProcessMessagesAsync<T, TH>(object sender, BasicDeliverEventArgs eventArgs)
       where T : Message
       where TH : IIntegrationEventHandler<T>
    {
        T integrationEvent;
        try
        {
            integrationEvent = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(eventArgs.Body.ToArray()));
        }
        catch
        {
            integrationEvent = Activator.CreateInstance<T>();
            integrationEvent.HasError = true;
        }

        integrationEvent.MessageToken = eventArgs.DeliveryTag.ToString(CultureInfo.InvariantCulture);

        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));

        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetService(typeof(TH));
            ((Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent })).GetAwaiter().GetResult();
        }
    }
}