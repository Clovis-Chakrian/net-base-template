using ChaCha.Core.Domain;
using ChaCha.Core.Extensions;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationTemplateParams;

namespace ChaCha.Notification.Domain.NotificationSentParamsValues;

public class NotificationSentParamValue : Entity<Guid>
{
    public Guid NotificationSentId { get; private set; }
    public Guid NotificationTemplateParamId { get; private set; }
    public string Value { get; private set; }

    public NotificationSent NotificationSent { get; private set; } = null!;
    public NotificationTemplateParam NotificationTemplateParam { get; private set; } = null!;

    private NotificationSentParamValue(Guid notificationSentId, Guid notificationTemplateParamId, string value) : base(Guid.NewGuid())
    {
        NotificationSentId = notificationSentId;
        NotificationTemplateParamId = notificationTemplateParamId;
        Value = value;
    }

    public static Result<NotificationSentParamValue> Create(Guid notificationSentId, Guid notificationTemplateParamId, string value)
    {
        var result = Result<NotificationSentParamValue>.Create();

        if (value.IsNullOrEmpty())
        {
            result.AddFailure("Value", "O valor do parâmetro é uma propriedade obrigatória.");
            return result;
        }

        var notificationSentParamValue = new NotificationSentParamValue(
            notificationSentId: notificationSentId,
            notificationTemplateParamId: notificationTemplateParamId,
            value: value
        );

        return result.Success(notificationSentParamValue);
    }
}