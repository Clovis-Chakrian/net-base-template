namespace ChaCha.MailerSend.SDK;

#nullable disable
public class MailerSendProperties
{
  public const string PropertiesSection = "MailerSendConfiguration";

  public string ClientBaseUrl { get; set; }
  public string ApiKey { get; set; }
}