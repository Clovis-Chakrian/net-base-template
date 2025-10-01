using ChaCha.Core.Exceptions;
using ChaCha.Core.Repositories;
using ChaCha.Data.Resolvers;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChaCha.Data.Persistence.Repositories.Cache;

public class CacheRepository : ICacheRepository
{
    private readonly IConnectionMultiplexer _redis;
    private const int DefaultExpirationInMinutes = 5;

    public CacheRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable("CACHE_DB_URL");

        if (string.IsNullOrEmpty(connectionString))
            throw new RuntimeException("CACHE_DB_URL env variable not found. Did you miss to setup?");

        _redis = ConnectionMultiplexer.Connect(connectionString);
    }

    public bool Set<T>(string key, T value, int? expirationInMinutes = null, string? keyPrefix = null)
    {
        if (value is null)
        {
            return false;
        }

        var db = _redis.GetDatabase();

        var serializedValue = Serialize(value);

        if (string.IsNullOrEmpty(serializedValue))
        {
            return false;
        }

        var finalKeyPrefix = GetFinalKeyPrefix(typeof(T), keyPrefix);
        var finalKey = $"{finalKeyPrefix}{key}";

        var couldSave = db.StringSet(
            key: finalKey,
            value: serializedValue,
            expiry: TimeSpan.FromMinutes(expirationInMinutes ?? DefaultExpirationInMinutes)
            );

        return couldSave;
    }

    public async Task<bool> SetAsync<T>(string key, T value, int? expirationInMinutes = null, string? keyPrefix = null)
    {
        if (value is null)
        {
            return false;
        }

        var db = _redis.GetDatabase();

        var serializedValue = Serialize(value);

        if (string.IsNullOrEmpty(serializedValue))
        {
            return false;
        }

        var finalKeyPrefix = GetFinalKeyPrefix(typeof(T), keyPrefix);
        var finalKey = $"{finalKeyPrefix}{key}";

        var couldSave = await db.StringSetAsync(
            key: finalKey,
            value: serializedValue,
            expiry: TimeSpan.FromMinutes(expirationInMinutes ?? DefaultExpirationInMinutes)
        );

        return couldSave;
    }

    public T? Get<T>(string key, string? keyPrefix = null)
    {
        var db = _redis.GetDatabase();

        var finalKeyPrefix = GetFinalKeyPrefix(typeof(T), keyPrefix);
        var finalKey = $"{finalKeyPrefix}{key}";

        var serializedValue = db.StringGet(finalKey);
        if (serializedValue.IsNullOrEmpty)
        {
            return default;
        }

        var deserializedValue = Deserialize<T>(serializedValue!);

        return deserializedValue;
    }

    public async Task<T?> GetAsync<T>(string key, string? keyPrefix = null)
    {
        var db = _redis.GetDatabase();

        var finalKeyPrefix = GetFinalKeyPrefix(typeof(T), keyPrefix);
        var finalKey = $"{finalKeyPrefix}{key}";

        var serializedValue = await db.StringGetAsync(finalKey);
        if (serializedValue.IsNullOrEmpty)
        {
            return default;
        }

        var deserializedValue = Deserialize<T>(serializedValue!);

        return deserializedValue;
    }

    public T? GetAndDelete<T>(string key, string? keyPrefix = null)
    {
        var db = _redis.GetDatabase();

        var finalKeyPrefix = GetFinalKeyPrefix(typeof(T), keyPrefix);
        var finalKey = $"{finalKeyPrefix}{key}";

        var serializedValue = db.StringGetDelete(finalKey);
        if (serializedValue.IsNullOrEmpty)
        {
            return default;
        }

        var deserializedValue = Deserialize<T>(serializedValue!);

        return deserializedValue;
    }

    public async Task<T?> GetAndDeleteAsync<T>(string key, string? keyPrefix = null)
    {
        var db = _redis.GetDatabase();

        var finalKeyPrefix = GetFinalKeyPrefix(typeof(T), keyPrefix);
        var finalKey = $"{finalKeyPrefix}{key}";

        var serializedValue = await db.StringGetDeleteAsync(finalKey);
        if (serializedValue.IsNullOrEmpty)
        {
            return default;
        }

        var deserializedValue = Deserialize<T>(serializedValue!);

        return deserializedValue;
    }

    private string? Serialize<T>(T value)
    {
        var serializeObject = JsonConvert.SerializeObject(value, SerializeConfig());
        return serializeObject;
    }

    private T? Deserialize<T>(string value)
    {
        var serializeObject = JsonConvert.DeserializeObject<T>(value, SerializeConfig());
        return serializeObject;
    }

    private string GetFinalKeyPrefix(Type value, string? keyPrefix)
    {
        if (keyPrefix != null)
        {
            return $"{keyPrefix}_";
        }

        var typeGenericArgsNames = string.Join("_", value.GenericTypeArguments.AsEnumerable().Select(arg => arg.Name));
        var typeName = value.Name;

        return $"{typeName}_{typeGenericArgsNames}_";
    }

    private static JsonSerializerSettings SerializeConfig()
    {
        return new JsonSerializerSettings()
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}