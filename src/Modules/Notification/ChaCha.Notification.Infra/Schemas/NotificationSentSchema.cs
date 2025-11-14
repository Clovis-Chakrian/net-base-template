using ChaCha.Core.Extensions;
using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationTemplates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationSentSchema : BaseSchema<NotificationSent, Guid>
{
  public override void Configure(EntityTypeBuilder<NotificationSent> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.NotificationExternalProviderRequestId)
      .HasMaxLength(30);

    builder.Property(p => p.Status)
      .HasConversion(
        new ValueConverter<ENotificationStatus, string>(
          v => v.GetDescription(),
          v => EnumExtensions.FromDescription<ENotificationStatus>(v)
        ))
      .HasMaxLength(25);

    builder.HasOne(p => p.NotificationTemplate)
      .WithMany()
      .HasForeignKey(p => p.NotificationTemplateId);

    builder.HasOne(p => p.NotificationType)
      .WithMany()
      .HasForeignKey(p => p.NotificationTypeId);

    builder.HasOne(p => p.NotificationChannel)
      .WithMany(p => p.NotificationSents)
      .HasForeignKey(p => p.NotificationChannelId);

    builder.HasOne(p => p.NotificationExternalProvider)
      .WithMany(p => p.NotificationSents)
      .HasForeignKey(p => p.NotificationExternalProviderId);

    builder.HasMany(p => p.ReasonsNotSent)
      .WithOne(p => p.NotificationSent);

    builder.HasMany(p => p.NotificationSentParamValues)
      .WithOne(p => p.NotificationSent);
  }
}