using ChaCha.MediatR.DomainEvents;

namespace ChaCha.Core.Domain;

public abstract class Entity<TKey> : IEntity
{
  public TKey Id { get; private set; }
  public DateTime CreatedAt { get; protected set; }
  public DateTime UpdatedAt { get; protected set; }
  private IEnumerable<IDomainEvent> _domainEvents = [];

  protected Entity(TKey key)
  {
    Id = key;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
  }

  protected void AddDomainEvent(IDomainEvent domainEvent)
  {
    _domainEvents = _domainEvents.Append(domainEvent);
  }

  public void ClearDomainEvents()
  {
    _domainEvents = [];
  }

  public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
  {
    return _domainEvents.ToList().AsReadOnly();
  }

  public virtual void OnPreCommit() {}
}