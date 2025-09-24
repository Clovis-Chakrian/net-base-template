using ChaCha.MailerSend.SDK.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.MailerSend.SDK;

public static class MailerSenderDependencyInjection
{
  public static IServiceCollection AddMailerClient(this IServiceCollection service, IConfiguration configuration)
  {
    // service.Configure<MailerSendProperties>(configuration.GetSection(MailerSendProperties.PropertiesSection));
    service.AddScoped<IMailClient, MailClient>();
    return service;
  }
}