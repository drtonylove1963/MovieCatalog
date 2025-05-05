namespace MovieCatalog.Domain.Common;

// Base class for all aggregates
public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public int Version { get; protected set; }

    protected AggregateRoot() : base() { }
    
    protected AggregateRoot(Guid id) : base(id) { }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    protected void IncrementVersion()
    {
        Version++;
    }
}