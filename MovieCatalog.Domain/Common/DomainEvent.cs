namespace MovieCatalog.Domain.Common;

// Base class for all domain events
public abstract class DomainEvent
{
    public Guid Id { get; }
    public DateTime Timestamp { get; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
    }
}