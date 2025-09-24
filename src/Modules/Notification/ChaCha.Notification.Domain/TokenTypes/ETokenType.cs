using System.ComponentModel;

namespace ChaCha.Notification.Domain.TokenTypes;

public enum ETokenType
{
  [Description("Multi Factor Authentication")]
  MFA = 1,

  [Description("One Time Password")]
  OTP,

  [Description("Recover Account Verification Code")]
  RecoverAccount,
}