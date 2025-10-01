using ChaCha.Core.Repositories;

namespace ChaCha.Notification.Domain.NotificationsSent.Repositories;

public interface INotificationSentWriteRepository : IWriteRepository<NotificationSent, Guid>
{
    
}