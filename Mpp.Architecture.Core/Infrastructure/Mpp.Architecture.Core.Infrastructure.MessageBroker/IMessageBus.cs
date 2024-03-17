namespace Mpp.Architecture.Core.Infrastructure.MessageBroker;

public interface IMessageBus
{
    void Publish(Message message);

    void Publish(IEnumerable<Message> message);

    void RegisterHandler<T, TH>()
         where T : Message
         where TH : IIntegrationEventHandler<T>;

    Task CompleteAsync(string messageToken);
}