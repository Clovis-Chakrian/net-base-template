using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.TokensSent;
using ChaCha.Notification.Domain.TokenTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class TokenTypeSchema : BaseSchema<TokenType, ETokenType>
{
  public override void Configure(EntityTypeBuilder<TokenType> builder)
  {
    base.Configure(builder);

    builder.Property(e => e.Name)
      .IsRequired()
      .HasMaxLength(100);

    builder.HasIndex(e => e.Name)
      .IsUnique(true);

    builder.HasData(
      [
        TokenType.Create(id: ETokenType.MFA, name: "MFA", description: "Token used to multi factor authentication", expirationTime: 5, maxAttempts: 5, resentLimit: 5, tokenLength: 6),

        TokenType.Create(id: ETokenType.OTP, name: "OTP", description: "One Time Password token", expirationTime: 5, maxAttempts: 5, resentLimit: 5, tokenLength: 9),

        TokenType.Create(id: ETokenType.RecoverAccount, name: "RecoverAccount", description: "Recover account token", expirationTime: 5, maxAttempts: 5, resentLimit: 5, tokenLength: 6),
      ]
    );
  }
}