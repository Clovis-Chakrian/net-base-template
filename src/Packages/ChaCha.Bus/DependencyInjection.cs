using ChaCha.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Bus;

public static class DependencyInjection
{
  public static IServiceCollection AddMessageBus(this IServiceCollection services)
  {
    services.AddSingleton<IMessageBus>(sp => new MessageBus("host=localhost:5672;publisherConfirms=true;timeout=10;username=guest;password=guest"));
    return services;
  }
}