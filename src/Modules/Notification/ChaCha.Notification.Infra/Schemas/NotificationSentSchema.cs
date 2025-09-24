using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationsSent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationSentSchema : BaseSchema<NotificationSent, Guid>
{
  public override void Configure(EntityTypeBuilder<NotificationSent> builder)
  {
    base.Configure(builder);
    
    builder.Property(e => e.MessageId)
      .IsRequired(false)
      .HasMaxLength(255);

    builder.Property(e => e.MessageTemplateId)
      .IsRequired()
      .HasMaxLength(255);

    builder.Property(e => e.SentDate)
      .IsRequired(false);

    builder.Property(e => e.Personalizations)
      .IsRequired()
      .HasColumnType("jsonb");

    builder.Property(e => e.NotificationProvider)
      .IsRequired()
      .HasConversion<string>();

    builder.Property(e => e.NotificationMethod)
      .IsRequired()
      .HasConversion<string>();

    builder.HasIndex(e => e.MessageId)
      .IsUnique(true);
  }
}