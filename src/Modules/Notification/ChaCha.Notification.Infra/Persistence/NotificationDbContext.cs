using ChaCha.Data.Persistence;
using ChaCha.MediatR.Mediator;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.TokensSent;
using ChaCha.Notification.Domain.TokenTypes;
using Microsoft.EntityFrameworkCore;

namespace ChaCha.Notification.Infra.Persistence;

public class NotificationDbContext : BaseDbContext
{
  public NotificationDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
  {
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
    builder.HasDefaultSchema("notification");
  }

  public DbSet<NotificationSent> NotificationsSent { get; set; }
  public DbSet<TokenSent> TokensSent { get; set; }
  public DbSet<TokenType> TokenTypes { get; set; }
}