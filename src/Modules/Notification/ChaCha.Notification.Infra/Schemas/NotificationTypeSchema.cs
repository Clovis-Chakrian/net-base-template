using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.NotificationTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class NotificationTypeSchema : BaseSchema<NotificationType, int>
{
  public override void Configure(EntityTypeBuilder<NotificationType> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.Name)
      .HasMaxLength(30);

    builder.Property(p => p.Description)
      .HasMaxLength(250);

    builder.HasMany(p => p.NotificationSents)
      .WithOne(p => p.NotificationType);

    builder.HasMany(p => p.NotificationTemplates)
      .WithOne(p => p.NotificationType);

    builder.HasData(
      NotificationType.Create(ENotificationType.System, "System", "Notificações relacionadas ao sistema e possíveis falhas que ocorram.").Data!,
      NotificationType.Create(ENotificationType.Trasactional, "Transactional", "Notificações sobre ocorrências do sistema para os usuários.").Data!
    );
  }
}