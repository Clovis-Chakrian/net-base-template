using ChaCha.Core.Domain;
using ChaCha.Core.Extensions;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationsSent;

namespace ChaCha.Notification.Domain.NotificationReasonsNotSent;

public class NotificationReasonNotSent : Entity<Guid>
{
  public Guid NotificationSentId { get; private set; }
  public string Reason { get; private set; }

  public NotificationSent NotificationSent { get; private set; } = null!;

  private NotificationReasonNotSent() : base(Guid.NewGuid())
  { }

  private NotificationReasonNotSent(Guid notificationSentId, string reason) : base(Guid.NewGuid())
  {
    NotificationSentId = notificationSentId;
    Reason = reason;
  }

  public static Result<NotificationReasonNotSent> Create(Guid notificationSentId, string reason)
  {
    var result = Result<NotificationReasonNotSent>.Create();

    if (reason.IsNullOrEmpty())
    {
      result.AddFailure("Reason", "A razão é uma propriedade obrigatória e não pode ser nula ou vazia.");
      return result;
    }

    var reasonNotSent = new NotificationReasonNotSent(
      notificationSentId: notificationSentId,
      reason: reason
      );

    return result.Success(reasonNotSent);
  }
}
