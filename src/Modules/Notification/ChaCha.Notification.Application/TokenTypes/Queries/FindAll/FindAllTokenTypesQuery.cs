using ChaCha.Core.Repositories.Pagination;
using ChaCha.MediatR.Queries;
using ChaCha.Notification.Domain.TokenTypes;

namespace ChaCha.Notification.Application.TokenTypes.Queries.FindAll;

public record FindAllTokenTypesQuery() : IQuery<Page<TokenTypeListDto>>;