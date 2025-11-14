using ChaCha.Data.Persistence;
using ChaCha.MediatR.Mediator;
using ChaCha.Notification.Domain.NotificationChannels;
using ChaCha.Notification.Domain.NotificationExternalProviders;
using ChaCha.Notification.Domain.NotificationReasonsNotSent;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationTemplates;
using ChaCha.Notification.Domain.NotificationTypes;
using Microsoft.EntityFrameworkCore;

namespace ChaCha.Notification.Infra.Persistence;

public class NotificationDbContext : ApplicationDbContext
{
  public NotificationDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
    modelBuilder.HasDefaultSchema("notification");
  }

  public DbSet<NotificationSent> NotificationsSent { get; set; }
  public DbSet<NotificationChannel> NotificationChannels { get; set; }
  public DbSet<NotificationExternalProvider> NotificationExternalProviders { get; set; }
  public DbSet<NotificationReasonNotSent> NotificationReasonsNotSent { get; set; }
  public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
  public DbSet<NotificationType> NotificationTypes { get; set; }
}