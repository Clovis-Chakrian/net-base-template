using ChaCha.Core.Repositories.Pagination;
using ChaCha.MediatR;
using ChaCha.MediatR.Queries;
using ChaCha.Notification.Domain.TokenTypes;
using ChaCha.Notification.Domain.TokenTypes.Repositories;

namespace ChaCha.Notification.Application.TokenTypes.Queries.FindAll;

public class FindAllTokenTypesQueryHandler : IQueryHandler<FindAllTokenTypesQuery, Page<TokenTypeListDto>>
{
  private readonly ITokenTypeReadRepository _tokenTypeReadRepository;

  public FindAllTokenTypesQueryHandler(ITokenTypeReadRepository tokenTypeReadRepository)
  {
    _tokenTypeReadRepository = tokenTypeReadRepository;
  }

  public async Task<Result<Page<TokenTypeListDto>>> Handle(FindAllTokenTypesQuery query, CancellationToken cancellationToken)
  {
    var result = Result<Page<TokenTypeListDto>>.Create();

    var queryResult = await _tokenTypeReadRepository.FindAllAsync();
    var mappedQuery = queryResult.Items.Select(tokenType => new TokenTypeListDto(tokenType));
    var pageReturn = new Page<TokenTypeListDto>(
          items: mappedQuery,
          pageInformation: queryResult.PageInformation
          );

    return result.Success(pageReturn);
  }
}