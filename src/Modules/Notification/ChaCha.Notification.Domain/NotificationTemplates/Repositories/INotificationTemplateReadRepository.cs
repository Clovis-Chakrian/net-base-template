using ChaCha.Core.Repositories;

namespace ChaCha.Notification.Domain.NotificationTemplates.Repositories;

public interface INotificationTemplateReadRepository : IReadRepository<NotificationTemplate, Guid>
{ }