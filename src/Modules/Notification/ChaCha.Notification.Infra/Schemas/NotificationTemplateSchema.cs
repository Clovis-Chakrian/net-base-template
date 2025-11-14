using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationTemplates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationTemplateSchema : BaseSchema<NotificationTemplate, Guid>
{
  public override void Configure(EntityTypeBuilder<NotificationTemplate> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.ExternalTemplateId)
      .HasMaxLength(60);

    builder.Property(p => p.Subject)
      .HasMaxLength(100);

    builder.Property(p => p.Body)
      .HasMaxLength(3000);

    builder.HasOne(p => p.NotificationExternalProvider)
      .WithMany(p => p.NotificationTemplates)
      .HasForeignKey(p => p.NotificationExternalProviderId);

    builder.HasOne(p => p.NotificationType)
      .WithMany(p => p.NotificationTemplates)
      .HasForeignKey(p => p.NotificationTypeId);

    builder.HasOne(p => p.NotificationChannel)
      .WithMany(p => p.NotificationTemplates)
      .HasForeignKey(p => p.NotificationChannelId);
  }
}