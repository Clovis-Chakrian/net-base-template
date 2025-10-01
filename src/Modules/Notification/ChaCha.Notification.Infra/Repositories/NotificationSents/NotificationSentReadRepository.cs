using ChaCha.Core.Repositories;
using ChaCha.Data.Persistence.Repositories.Read;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationsSent.Repositories;
using ChaCha.Notification.Infra.Persistence;

namespace ChaCha.Notification.Infra.Repositories.NotificationSents;

public class NotificationSentReadRepository : ReadRepository<NotificationSent, Guid, NotificationDbContext>, INotificationSentReadRepository
{
    public NotificationSentReadRepository(NotificationDbContext context, ICacheRepository cacheRepository) : base(context, cacheRepository)
    {
    }
}