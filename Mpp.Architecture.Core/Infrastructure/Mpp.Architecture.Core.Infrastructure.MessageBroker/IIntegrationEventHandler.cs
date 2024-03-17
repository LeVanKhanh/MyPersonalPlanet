namespace Mpp.Architecture.Core.Infrastructure.MessageBroker
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
      where TIntegrationEvent : Message
    {
        Task Handle(TIntegrationEvent integrationEvent);
    }

    public interface IIntegrationEventHandler
    {

    }
}
