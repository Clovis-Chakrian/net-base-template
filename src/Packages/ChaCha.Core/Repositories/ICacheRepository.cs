namespace ChaCha.Core.Repositories;

public interface ICacheRepository
{
    bool Set<T>(string key, T value, int? expirationInMinutes = null, string? keyPrefix = null);
    Task<bool> SetAsync<T>(string key, T value, int? expirationInMinutes = null, string? keyPrefix = null);
    public T? Get<T>(string key, string? keyPrefix = null);
    public Task<T?> GetAsync<T>(string key, string? keyPrefix = null);
    public T? GetAndDelete<T>(string key, string? keyPrefix = null);
    Task<T?> GetAndDeleteAsync<T>(string key, string? keyPrefix = null);
}