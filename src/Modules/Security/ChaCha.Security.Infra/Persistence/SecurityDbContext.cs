using Microsoft.EntityFrameworkCore;
using ChaCha.Security.Domain.Users;
using ChaCha.MediatR.Mediator;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ChaCha.Core.Domain;

namespace ChaCha.Security.Infra.Persistence;

public class SecurityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{

  private readonly IMediator _mediator;

  public SecurityDbContext(DbContextOptions<SecurityDbContext> options, IMediator mediator) : base(options)
  {
    _mediator = mediator;
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.ApplyConfigurationsFromAssembly(typeof(SecurityDbContext).Assembly);
    builder.HasDefaultSchema("security");
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
      .Where(e => (e.Entity is IEntity) && (e.State == EntityState.Added || e.State == EntityState.Modified));

    var entities = entries
        .Select(e => e.Entity)
        .OfType<IEntity>();

    var domainEvents = entities
      .SelectMany(p => p.GetDomainEvents())
      .ToList();

    foreach (var entry in entries)
    {
      entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;

      if (entry.State == EntityState.Added)
      {
        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
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