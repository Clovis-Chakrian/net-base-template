using ChaCha.Data.Persistence.Repositories.Write;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationsSent.Repositories;
using ChaCha.Notification.Infra.Persistence;

namespace ChaCha.Notification.Infra.Repositories.NotificationSents;

public class NotificationSentWriteRepository : WriteRepository<NotificationSent, Guid, NotificationDbContext>, INotificationSentWriteRepository
{
    public NotificationSentWriteRepository(NotificationDbContext context) : base(context)
    {
    }
}