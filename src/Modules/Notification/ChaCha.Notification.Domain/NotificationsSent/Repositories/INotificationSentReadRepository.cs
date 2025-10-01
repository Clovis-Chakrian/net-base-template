using ChaCha.Core.Repositories;

namespace ChaCha.Notification.Domain.NotificationsSent.Repositories;

public interface INotificationSentReadRepository : IReadRepository<NotificationSent, Guid>
{
    
}