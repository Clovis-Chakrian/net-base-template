using ChaCha.Core.Domain;
using ChaCha.MediatR.DomainEvents;
using Microsoft.AspNetCore.Identity;

namespace ChaCha.Auth.Persistence;

public class ApplicationUser : IdentityUser<Guid>, IEntity
{
  public DateTime CreatedAt { get; protected set; }
  public DateTime UpdatedAt { get; protected set; }
  private IEnumerable<IDomainEvent> _domainEvents = [];

  protected ApplicationUser()
  {
    Id = Guid.NewGuid();
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
  }

  protected void AddDomainEvent(IDomainEvent domainEvent)
  {
    _domainEvents = _domainEvents.Append(domainEvent);
  }

  public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
  {
    return _domainEvents.ToList().AsReadOnly();
  }

  public void ClearDomainEvents()
  {
    _domainEvents = [];
  }

  public virtual void OnPreCommit() {}
}