using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationExternalProviders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationExternalProviderSchema : BaseSchema<NotificationExternalProvider, int>
{
  public override void Configure(EntityTypeBuilder<NotificationExternalProvider> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.Name)
      .HasMaxLength(30);

    builder.Property(p => p.Description)
      .HasMaxLength(250);

    builder.HasMany(p => p.NotificationSents)
      .WithOne(p => p.NotificationExternalProvider);

    builder.HasMany(p => p.NotificationTemplates)
      .WithOne(p => p.NotificationExternalProvider);

    builder.HasData(
      NotificationExternalProvider.Create(ENotificationExternalProvider.Mailgun, "Mailgun", "Provider utilizado para envio de Email's.").Data!,
      NotificationExternalProvider.Create(ENotificationExternalProvider.Firebase, "Firebase", "Provider utilizado para envio de push notifications.").Data!,
      NotificationExternalProvider.Create(ENotificationExternalProvider.Twillio, "Twillio", "Provider utilizado para envio de WhatsApp ou SMS.").Data!
    );
  }
}