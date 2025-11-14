using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationTemplateParams;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationTemplateParamSchema : BaseSchema<NotificationTemplateParam, Guid>
{
    public override void Configure(EntityTypeBuilder<NotificationTemplateParam> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
            .HasMaxLength(150);

        builder.Property(p => p.Description)
            .HasMaxLength(250);

        builder
            .HasOne(p => p.NotificationTemplate)
            .WithMany(p => p.NotificationTemplateParams)
            .HasForeignKey(p => p.NotificationTemplateId);

        builder.HasMany(p => p.NotificationSentParamValues)
            .WithOne(p => p.NotificationTemplateParam);
    }
}