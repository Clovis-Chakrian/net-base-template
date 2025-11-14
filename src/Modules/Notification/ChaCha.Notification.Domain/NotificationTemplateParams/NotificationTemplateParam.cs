using ChaCha.Core.Domain;
using ChaCha.Core.Extensions;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationSentParamsValues;
using ChaCha.Notification.Domain.NotificationTemplates;

namespace ChaCha.Notification.Domain.NotificationTemplateParams;

public class NotificationTemplateParam : Entity<Guid>
{
  public string Name { get; private set; }
  public string Description { get; private set; }
  public Guid NotificationTemplateId { get; private set; }

  public NotificationTemplate? NotificationTemplate { get; private set; } = null;
  public IEnumerable<NotificationSentParamValue> NotificationSentParamValues { get; private set; } = [];

  private NotificationTemplateParam(string name, string description, Guid notificationTemplateId) : base(Guid.NewGuid())
  {
    Name = name;
    Description = description;
    NotificationTemplateId = notificationTemplateId;
  }

  public static Result<NotificationTemplateParam> Create(string name, string description, Guid notificationTemplateId)
  {
    var result = Result<NotificationTemplateParam>.Create();

    if (name.IsNullOrEmpty())
    {
      result.AddFailure("Name", "A propriedade nome é obrigatória.");
      return result;
    }

    if (description.IsNullOrEmpty())
    {
      result.AddFailure("Name", "A propriedade descrição é obrigatória.");
      return result;
    }

    var notificationTemplateParam = new NotificationTemplateParam(
      name: name,
      description: description,
      notificationTemplateId: notificationTemplateId
      );

    return result.Success(notificationTemplateParam);
  }
}