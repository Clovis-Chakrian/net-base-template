using ChaCha.Core.Domain;
using ChaCha.Core.Extensions;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationTemplates;

namespace ChaCha.Notification.Domain.NotificationChannels;

public class NotificationChannel : Entity<int>
{
  public string Name { get; private set; }
  public string? Description { get; private set; }
  public IEnumerable<NotificationSent> NotificationSents { get; private set; } = [];
  public IEnumerable<NotificationTemplate> NotificationTemplates { get; private set; } = [];

  private NotificationChannel() : base(1)
  {
  }

  private NotificationChannel(ENotificationChannel notificationChannel, string name, string? description) : base((int)notificationChannel)
  {
    Name = name;
    Description = description;
  }

  public static Result<NotificationChannel> Create(ENotificationChannel notificationChannel, string name, string? description)
  {
    var result = Result<NotificationChannel>.Create();

    if (name.IsNullOrEmpty())
    {
      result.AddFailure("Name", "A propriedade nome é obrigatória.");
    }

    if (description.IsNullOrEmpty())
    {
      result.AddFailure("Description", "A propriedade description é obrigatória.");
    }

    var notificationChannelEntity = new NotificationChannel(
      notificationChannel: notificationChannel,
      name: name,
      description: description
      );

    return result.Success(notificationChannelEntity);
  }
}