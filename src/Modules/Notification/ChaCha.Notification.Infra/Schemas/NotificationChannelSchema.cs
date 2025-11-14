using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationChannels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationChannelSchema : BaseSchema<NotificationChannel, int>
{
  public override void Configure(EntityTypeBuilder<NotificationChannel> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.Name)
      .HasMaxLength(30);

    builder.Property(p => p.Description)
      .HasMaxLength(250);

    builder.HasMany(p => p.NotificationSents)
      .WithOne(p => p.NotificationChannel);

    builder.HasMany(p => p.NotificationTemplates)
      .WithOne(p => p.NotificationChannel);

    builder.HasData(
      NotificationChannel.Create(ENotificationChannel.Email, "Email", "Representa o canal de envio de emails.").Data!,
      NotificationChannel.Create(ENotificationChannel.WhatsApp, "WhatsApp", "Representa o canal de envio de whatsapps.").Data!,
      NotificationChannel.Create(ENotificationChannel.Sms, "SMS", "Representa o canal de envio de SMS's.").Data!,
      NotificationChannel.Create(ENotificationChannel.Push, "Push", "Representa o canal de envio de notificações push.").Data!
      );
  }
}