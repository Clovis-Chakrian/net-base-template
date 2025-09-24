using System.Security.Cryptography;
using System.Text;
using ChaCha.Core.Domain;
using ChaCha.MediatR;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.TokenTypes;

namespace ChaCha.Notification.Domain.TokensSent;

public sealed class TokenSent : Entity<Guid>
{
  public string Token { get; private set; }
  public ETokenType TokenTypeId { get; private set; }
  public Guid NotificationSentId { get; private set; }
  public bool Used { get; private set; }
  public int Attempts { get; private set; }
  public DateTime ExpiresAt { get; private set; }
  public string Salt { get; private set; }
  public string DataCheck { get; private set; }
  public DateTime? UsedAt { get; private set; }

  public NotificationSent? NotificationSent { get; private set; }
  public TokenType? TokenType { get; private set; }

  private TokenSent() : base(Guid.NewGuid())
  {
    Token = string.Empty;
    Salt = string.Empty;
    DataCheck = string.Empty;
  }

  private TokenSent(TokenType tokenType, string dataCheck) : base(Guid.NewGuid())
  {
    Token = GenerateToken(tokenType);
    TokenType = tokenType;
    TokenTypeId = tokenType.Id;
    ExpiresAt = DateTime.UtcNow.AddMinutes(TokenType.ExpirationTime);
    Salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
    DataCheck = dataCheck;
    Used = false;
    Attempts = 0;
  }

  public static Result<TokenSent> Create(TokenType tokenType, string dataCheck)
  {
    var result = Result<TokenSent>.Create();
    return result.Success(new TokenSent(tokenType: tokenType, dataCheck: dataCheck));
  }

  public bool CanBeUsed()
  {
    return !Used &&
      Attempts < TokenType?.MaxAttempts &&
      DateTime.UtcNow <= ExpiresAt;
  }

  public Result Verify(string tokenToCompare)
  {
    var result = Result.Create();

    if (!CanBeUsed())
    {
      result.AddFailure("Token cannot be used");
    }

    var hashedTokenToCompare = ComputeHash(tokenToCompare);
    var hashedToken = Convert.FromBase64String(Token);
    var isValid = CryptographicOperations.FixedTimeEquals(hashedToken, hashedTokenToCompare);

    if (isValid)
    {
      Used = true;
      UsedAt = DateTime.UtcNow;
      return result.Success();
    }

    Attempts++;
    result.AddFailure("Invalid token");
    return result;
  }

  public override void OnPreCommit()
  {
    EncryptToken();
  }

  private string GenerateToken(TokenType tokenType)
  {
    var length = tokenType?.TokenLength ?? 6;
    var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".AsSpan();
    var otp = new StringBuilder(length);

    for (int i = 0; i < length; i++)
    {
      int idx = RandomNumberGenerator.GetInt32(chars.Length);
      otp.Append(chars[idx]);
    }

    return otp.ToString();
  }

  private void EncryptToken()
  {
    var hashedToken = ComputeHash(Token);
    Token = Convert.ToBase64String(hashedToken);
  }

  private byte[] ComputeHash(string token)
  {
    var msg = Encoding.UTF8.GetBytes($"{DataCheck}|{TokenType?.Name}|{Salt}|{token}");
    var pepper = Encoding.UTF8.GetBytes(Id.ToString());
    using var hmac = new HMACSHA256(pepper);
    return hmac.ComputeHash(msg);
  }
}