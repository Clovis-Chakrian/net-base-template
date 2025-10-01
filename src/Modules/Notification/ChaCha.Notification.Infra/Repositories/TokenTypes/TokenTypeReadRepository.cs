using ChaCha.Core.Repositories;
using ChaCha.Data.Persistence.Repositories.Read;
using ChaCha.Notification.Domain.TokenTypes;
using ChaCha.Notification.Domain.TokenTypes.Repositories;
using ChaCha.Notification.Infra.Persistence;

namespace ChaCha.Notification.Infra.Repositories.TokenTypes;

public class TokenTypeReadRepository : ReadRepository<TokenType, ETokenType, NotificationDbContext>, ITokenTypeReadRepository
{
    public TokenTypeReadRepository(NotificationDbContext context, ICacheRepository cacheRepository) : base(context, cacheRepository)
    {
    }
}