namespace Mpp.Architecture.Core.Domain.Entities
{
    using MediatR;
    public abstract class Entity
    {
        protected List<INotification>? _domainEvents;
        public List<INotification>? DomainEvents => _domainEvents;
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        public void RemoveDomainEvent(INotification eventItem)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(eventItem);
        }
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }

    public abstract class Entity<T> : Entity
        where T : struct
    {
        protected Entity()
        {

        }

        protected Entity(T id)
        {
            Id = id;
        }

        public T Id { get; protected set; }
    }
}
