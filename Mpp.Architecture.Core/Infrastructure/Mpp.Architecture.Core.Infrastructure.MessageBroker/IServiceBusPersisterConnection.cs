namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

using Microsoft.Azure.ServiceBus;

public interface IServiceBusPersisterConnection : IDisposable
{
    ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }
    IQueueClient CreateModel();
}