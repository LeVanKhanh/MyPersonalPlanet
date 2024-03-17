namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;
using RabbitMQ.Client;
public interface IRabbitMQPersistentConnection
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();

    string GetQueue();
}