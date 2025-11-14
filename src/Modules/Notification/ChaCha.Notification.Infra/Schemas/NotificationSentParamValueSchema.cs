using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationSentParamsValues;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationSentParamValueSchema : BaseSchema<NotificationSentParamValue, Guid>
{
    public override void Configure(EntityTypeBuilder<NotificationSentParamValue> builder)
    {
        base.Configure(builder);
        
        builder.Property(p => p.Value)
            .HasMaxLength(350);

        builder.HasOne(p => p.NotificationSent)
            .WithMany(p => p.NotificationSentParamValues)
            .HasForeignKey(p => p.NotificationSentId);

        builder.HasOne(p => p.NotificationTemplateParam)
            .WithMany(p => p.NotificationSentParamValues)
            .HasForeignKey(p => p.NotificationTemplateParamId);
    }
}