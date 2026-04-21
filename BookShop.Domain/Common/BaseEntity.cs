namespace BookShop.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id {get;protected set;}
        public DateTime CreatedAt {get; protected set;}
        public DateTime UpdatedAt {get; protected set;}

        // Domain Events
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvent()
        {
            _domainEvents.Clear();
        }

    }
}