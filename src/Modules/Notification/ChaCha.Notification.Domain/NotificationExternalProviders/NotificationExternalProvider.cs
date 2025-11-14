using ChaCha.Core.Domain;
using ChaCha.Core.Extensions;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationTemplates;

namespace ChaCha.Notification.Domain.NotificationExternalProviders;

public class NotificationExternalProvider : Entity<int>
{
  public string Name { get; private set; }
  public string? Description { get; private set; }

  public IEnumerable<NotificationSent> NotificationSents { get; private set; } = [];
  public IEnumerable<NotificationTemplate> NotificationTemplates { get; private set; } = [];

  private NotificationExternalProvider() : base(1)
  { }

  private NotificationExternalProvider(ENotificationExternalProvider notificationExternalProvider, string name, string? description) : base((int)notificationExternalProvider)
  {
    Name = name;
    Description = description;
  }

  public static Result<NotificationExternalProvider> Create(ENotificationExternalProvider notificationExternalProvider, string name, string? description)
  {
    var result = Result<NotificationExternalProvider>.Create();

    if (name.IsNullOrEmpty())
    {
      result.AddFailure("Name", "A propriedade nome é obrigatória.");
    }

    if (description.IsNullOrEmpty())
    {
      result.AddFailure("Description", "A propriedade description é obrigatória.");
    }

    var notificationExternalProviderEntity = new NotificationExternalProvider(
      notificationExternalProvider: notificationExternalProvider,
      name: name,
      description: description
      );

    return result.Success(data: notificationExternalProviderEntity);
  }
}