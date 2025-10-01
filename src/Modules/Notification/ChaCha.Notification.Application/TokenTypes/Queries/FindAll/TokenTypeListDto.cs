using ChaCha.Notification.Domain.TokenTypes;

namespace ChaCha.Notification.Application.TokenTypes.Queries.FindAll;

public record TokenTypeListDto
{
  public ETokenType Id { get; private set; }
  public string Name { get; private set; }
  public string Description { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public TokenTypeListDto(TokenType tokenType)
  {
    Id = tokenType.Id;
    Name = tokenType.Name;
    Description = tokenType.Description;
    UpdatedAt = tokenType.UpdatedAt;
  }
}