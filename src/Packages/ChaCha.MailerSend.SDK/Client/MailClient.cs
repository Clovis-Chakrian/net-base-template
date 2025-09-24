using Microsoft.Extensions.Options;

namespace ChaCha.MailerSend.SDK.Client;

public class MailClient : IMailClient
{
  private readonly HttpClient _httpClient;

  public MailClient(HttpClient httpClient, IOptions<MailerSendProperties> mailerSenderOptions)
  {
    _httpClient = httpClient;
    _httpClient.BaseAddress = new Uri(mailerSenderOptions.Value.ClientBaseUrl);
    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {mailerSenderOptions.Value.ApiKey}");
  }
  public void Send()
  {
    Console.WriteLine(_httpClient.BaseAddress);
  }
}