using System.ComponentModel;

namespace ChaCha.Notification.Domain.NotificationMethods;

public enum ENotificationMethod
{
  [Description("EMAIL")]
  Email,

  [Description("SMS")]
  SMS,

  [Description("Push")]
  Push,

  [Description("WhatsApp")]
  WhatsApp
}