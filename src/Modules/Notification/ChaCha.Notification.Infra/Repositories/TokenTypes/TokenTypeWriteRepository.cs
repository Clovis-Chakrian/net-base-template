using ChaCha.Data.Persistence.Repositories.Write;
using ChaCha.Notification.Domain.TokenTypes;
using ChaCha.Notification.Domain.TokenTypes.Repositories;
using ChaCha.Notification.Infra.Persistence;

namespace ChaCha.Notification.Infra.Repositories.TokenTypes;

public class TokenTypeWriteRepository : WriteRepository<TokenType, ETokenType, NotificationDbContext>, ITokenTypeWriteRepository
{
    public TokenTypeWriteRepository(NotificationDbContext context) : base(context)
    {
    }
}