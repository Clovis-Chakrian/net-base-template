using ChaCha.Core.Domain;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationMethods;
using ChaCha.Notification.Domain.NotificationProviders;

namespace ChaCha.Notification.Domain.NotificationsSent;

public class NotificationSent : Entity<Guid>
{
  public string? MessageId { get; private set; }
  public string MessageTemplateId { get; private set; }
  public DateTime? SentDate { get; private set; }
  public string Personalizations { get; private set; }
  public ENotificationProvider NotificationProvider { get; private set; }
  public ENotificationMethod NotificationMethod { get; private set; }

  private NotificationSent(string messageTemplateId, string personalizations) : base(Guid.NewGuid())
  {
    MessageTemplateId = messageTemplateId;
    Personalizations = personalizations;
  }

  public static Result<NotificationSent> Create(string messageTemplateId, string personalizations)
  {
    var result = Result<NotificationSent>.Create();
    return result.Success(new NotificationSent(messageTemplateId: messageTemplateId, personalizations: personalizations));
  }
}