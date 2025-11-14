using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationReasonsNotSent;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationReasonNotSentSchema : BaseSchema<NotificationReasonNotSent, Guid>
{
  public override void Configure(EntityTypeBuilder<NotificationReasonNotSent> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.Reason)
      .HasMaxLength(500);

    builder.HasOne(p => p.NotificationSent)
      .WithMany(p => p.ReasonsNotSent)
      .HasForeignKey(p => p.NotificationSentId);
  }
}