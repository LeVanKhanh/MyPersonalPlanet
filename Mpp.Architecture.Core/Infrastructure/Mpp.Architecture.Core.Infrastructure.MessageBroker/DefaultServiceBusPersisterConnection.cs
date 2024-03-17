namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

using Microsoft.Azure.ServiceBus;

public sealed class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
{
    private IQueueClient _queueClient;

    public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder)
    {
        ServiceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
            throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
        _queueClient = new QueueClient(ServiceBusConnectionStringBuilder, ReceiveMode.PeekLock, RetryPolicy.Default);
    }

    public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

    public IQueueClient CreateModel()
    {
        if (_queueClient.IsClosedOrClosing)
        {
            _queueClient = new QueueClient(ServiceBusConnectionStringBuilder, ReceiveMode.PeekLock, RetryPolicy.Default);
        }

        return _queueClient;
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
           
        }
    }
}
