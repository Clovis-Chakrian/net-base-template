using ChaCha.Core.Domain;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationTemplates;

namespace ChaCha.Notification.Domain.NotificationTypes;

public class NotificationType : Entity<int>
{
  public string Name { get; private set; }
  public string? Description { get; private set; }

  public IEnumerable<NotificationSent>? NotificationSents { get; private set; } = null;
  public IEnumerable<NotificationTemplate>? NotificationTemplates { get; private set; } = null;

  private NotificationType() : base(1)
  {}

  private NotificationType(ENotificationType notificationType, string name, string? description) : base((int)notificationType)
  {
    Name = name;
    Description = description;
  }

  public static Result<NotificationType> Create(ENotificationType notificationType, string name, string? description)
  {
    var result = Result<NotificationType>.Create();

    var notificationTypeEntity = new NotificationType(
      notificationType: notificationType,
      name: name,
      description: description
      );

    return result.Success(notificationTypeEntity);
  }
}