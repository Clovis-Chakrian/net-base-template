using ChaCha.Core.Domain;
using ChaCha.MediatR.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChaCha.Data.Persistence;

public class BaseDbContext : DbContext
{
  private readonly IMediator _mediator;

  public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
  {
    _mediator = mediator;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSnakeCaseNamingConvention();

    // Enable for development environment only
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local")
    {
      optionsBuilder.EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information);
    }
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var entries = ChangeTracker
      .Entries()
      .Where(e => e.Entity is IEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

    var entities = entries
     .Select(e => e.Entity)
     .OfType<IEntity>();

    var domainEvents = entities
      .SelectMany(p => p.GetDomainEvents())
      .ToList();

    foreach (var entry in entries)
    {
      if (entry.State == EntityState.Modified)
      {
        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
      }


      if (entry.State == EntityState.Added)
      {
        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
      }

      if (entry.Entity is IEntity entity)
      {
        entity.OnPreCommit();
      }
    }

    var saveChangesResponse = await base.SaveChangesAsync(cancellationToken);

    if (saveChangesResponse <= 0)
    {
      return saveChangesResponse;
    }

    foreach (var domainEvent in domainEvents)
    {
      _mediator.PublishEvent(domainEvent, cancellationToken);
    }

    foreach (var entity in entities)
    {
      entity.ClearDomainEvents();
    }

    return saveChangesResponse;
  }
}