using ChaCha.Core.Domain;
using ChaCha.Core.Extensions;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationChannels;
using ChaCha.Notification.Domain.NotificationExternalProviders;
using ChaCha.Notification.Domain.NotificationTemplateParams;
using ChaCha.Notification.Domain.NotificationTypes;

namespace ChaCha.Notification.Domain.NotificationTemplates;

public class NotificationTemplate : Entity<Guid>
{
  public string? ExternalTemplateId { get; private set; }
  public string? Subject { get; private set; }
  public string? Body { get; private set; }

  public int NotificationExternalProviderId { get; private set; }
  public int NotificationTypeId { get; private set; }
  public int NotificationChannelId { get; private set; }

  public NotificationExternalProvider? NotificationExternalProvider { get; private set; } = null;
  public NotificationType? NotificationType { get; private set; } = null;
  public NotificationChannel? NotificationChannel { get; private set; } = null;
  public IEnumerable<NotificationTemplateParam> NotificationTemplateParams { get; set; }

  private NotificationTemplate() : base(Guid.NewGuid())
  {

  }

  private NotificationTemplate(
    string? externalTemplateId,
    string? subject,
    string? body,
    ENotificationExternalProvider notificationExternalProvider,
    ENotificationType notificationType,
    ENotificationChannel notificationChannel
    ) : base(Guid.NewGuid())
  {
    ExternalTemplateId = externalTemplateId;
    Subject = subject;
    Body = body;
    NotificationExternalProviderId = (int)notificationExternalProvider;
    NotificationTypeId = (int)notificationType;
    NotificationChannelId = (int)notificationChannel;
  }

  public static Result<NotificationTemplate> Create(
    string? externalTemplateId,
    string? subject,
    string? body,
    ENotificationExternalProvider notificationExternalProvider,
    ENotificationType notificationType,
    ENotificationChannel notificationChannel
    )
  {
    var result = Result<NotificationTemplate>.Create();

    if (externalTemplateId.IsNullOrEmpty() && (subject.IsNullOrEmpty() || body.IsNullOrEmpty()))
    {
      result.AddFailure("O id externo do template é obrigatório quando o subject ou body são vazios");
      return result;
    }

    var notificationTemplate = new NotificationTemplate(
      externalTemplateId: externalTemplateId,
      subject: subject,
      body: body,
      notificationExternalProvider: notificationExternalProvider,
      notificationType: notificationType,
      notificationChannel: notificationChannel
    );

    return result.Success(notificationTemplate);
  }
}