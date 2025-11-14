using ChaCha.Core.Repositories;
using ChaCha.Data.Persistence.Repositories.Read;
using ChaCha.Notification.Domain.NotificationTemplates;
using ChaCha.Notification.Domain.NotificationTemplates.Repositories;
using ChaCha.Notification.Infra.Persistence;

namespace ChaCha.Notification.Infra.Repositories.NotificationTemplates;

public class NotificationTemplateReadRepository : ReadRepository<NotificationTemplate, Guid, NotificationDbContext>, INotificationTemplateReadRepository
{
  public NotificationTemplateReadRepository(NotificationDbContext context, ICacheRepository cacheRepository) : base(context, cacheRepository)
  {

  }
}