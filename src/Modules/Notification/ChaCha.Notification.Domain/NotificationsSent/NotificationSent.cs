using System.Text.Json;
using ChaCha.Core.Domain;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationChannels;
using ChaCha.Notification.Domain.NotificationExternalProviders;
using ChaCha.Notification.Domain.NotificationReasonsNotSent;
using ChaCha.Notification.Domain.NotificationSentParamsValues;
using ChaCha.Notification.Domain.NotificationTemplates;
using ChaCha.Notification.Domain.NotificationTypes;

namespace ChaCha.Notification.Domain.NotificationsSent;

public class NotificationSent : Entity<Guid>
{
  public Guid NotificationTemplateId { get; set; }
  public int NotificationChannelId { get; set; }
  public int NotificationExternalProviderId { get; set; }
  public int NotificationTypeId { get; set; }
  public string? NotificationExternalProviderRequestId { get; set; }
  public Guid CorrelationId { get; set; }
  public ENotificationStatus Status { get; set; }
  public DateTime? SentTime { get; set; }
  public int Attempts { get; set; }
  public bool ShouldRetry { get; set; }
  public Guid UserId { get; set; }

  public NotificationTemplate? NotificationTemplate { get; set; }
  public NotificationType? NotificationType { get; set; }
  public NotificationChannel? NotificationChannel { get; set; }
  public NotificationExternalProvider? NotificationExternalProvider { get; set; }
  public IEnumerable<NotificationReasonNotSent>? ReasonsNotSent { get; set; }
  public IEnumerable<NotificationSentParamValue>? NotificationSentParamValues { get; set; }

  private NotificationSent() : base(key: Guid.NewGuid())
  { }

  private NotificationSent(
    Guid notificationTemplateId,
    ENotificationChannel notificationChannel,
    ENotificationExternalProvider notificationExternalProvider,
    ENotificationType notificationType,
    bool shouldRetry,
    Guid userId,
    Guid correlationId
  ) : base(key: Guid.NewGuid())
  {
    NotificationTemplateId = notificationTemplateId;
    NotificationChannelId = (int)notificationChannel;
    NotificationExternalProviderId = (int)notificationExternalProvider;
    NotificationTypeId = (int)notificationType;
    ShouldRetry = shouldRetry;
    UserId = userId;
    Attempts = 0;
    Status = ENotificationStatus.Pending;
    CorrelationId = correlationId;
  }

  public static Result<NotificationSent> Create(
    Guid notificationTemplateId,
    ENotificationChannel notificationChannel,
    ENotificationExternalProvider notificationExternalProvider,
    ENotificationType notificationType,
    bool shouldRetry,
    Guid userId,
    Guid correlationId
  )
  {
    var result = Result<NotificationSent>.Create();

    var notificationSent = new NotificationSent(
      notificationTemplateId: notificationTemplateId,
      notificationChannel: notificationChannel,
      notificationExternalProvider: notificationExternalProvider,
      notificationType: notificationType,
      shouldRetry: shouldRetry,
      userId: userId,
      correlationId: correlationId
    );

    return result.Success(data: notificationSent);
  }

  public void MarkAsSent()
  {
    Attempts++;
    SentTime = DateTime.UtcNow;
    Status = ENotificationStatus.Sent;
  }

  public void AddReasonNotSent(string reason)
  {
    var reasonNotSent = NotificationReasonNotSent.Create(notificationSentId: Id, reason: reason);

    if (!reasonNotSent.IsValid)
    {
      return;
    }

    ReasonsNotSent = ReasonsNotSent is null ? [reasonNotSent?.Data!] : [..ReasonsNotSent, reasonNotSent?.Data!];
    Status = ENotificationStatus.Failed;
    Attempts++;
  }

  public Result AddParam(Guid paramId, string value)
  {
    var result = Result.Create();

    var notificationSentParamValueResult = NotificationSentParamValue.Create(
      notificationSentId: Id,
      notificationTemplateParamId: paramId,
      value: value
    );

    if (!notificationSentParamValueResult.IsValid)
    {
      result.HandleResult(notificationSentParamValueResult.ValidationResult);

      return result;
    }

    NotificationSentParamValues = NotificationSentParamValues is null ? [notificationSentParamValueResult.Data!] : [..NotificationSentParamValues, notificationSentParamValueResult.Data!];

    return result.Success();
  }

  public void AddNotificationExternalProviderRequestId(string requestId)
  {
    NotificationExternalProviderRequestId = requestId;
  }
}