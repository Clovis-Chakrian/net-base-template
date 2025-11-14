
using ChaCha.Storage.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Storage;

public static class StorageDependencyInjection
{
  public static IServiceCollection AddStorage(this IServiceCollection service)
  {
    service.AddScoped<IS3Service, S3Service>();
    return service;
  }
}