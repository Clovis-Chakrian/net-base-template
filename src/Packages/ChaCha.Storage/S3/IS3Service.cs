namespace ChaCha.Storage.S3;

public interface IS3Service
{
  Task<string?> GetObject(string bucket, string key);
}