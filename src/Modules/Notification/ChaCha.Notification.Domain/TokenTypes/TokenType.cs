using ChaCha.Core.Domain;

namespace ChaCha.Notification.Domain.TokenTypes;

public sealed class TokenType : Entity<ETokenType>
{
  public string Name { get; private set; }
  public string Description { get; private set; }
  public int ExpirationTime { get; private set; }
  public int MaxAttempts { get; private set; }
  public int ResentLimit { get; private set; }
  public int TokenLength { get; private set; }

  private TokenType(ETokenType id, string name, string description, int expirationTime, int maxAttempts, int resentLimit, int tokenLength) : base(id)
  {
    Name = name;
    Description = description;
    ExpirationTime = expirationTime;
    MaxAttempts = maxAttempts;
    ResentLimit = resentLimit;
    TokenLength = tokenLength;
  }

  public static TokenType Create(ETokenType id, string name, string description, int expirationTime, int maxAttempts, int resentLimit, int tokenLength)
  {
    return new TokenType(id, name, description, expirationTime, maxAttempts, resentLimit, tokenLength);
  }
}