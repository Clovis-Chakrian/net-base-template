using Amazon.Runtime;
using Amazon.S3;

namespace ChaCha.Storage.S3;

public class S3Service : IS3Service
{
  private readonly AmazonS3Client _s3Client;
  public S3Service()
  {
    var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY") ?? "<ACCESS_KEY>";
    var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY") ?? "<SECRET_KEY>";
    var accountId = Environment.GetEnvironmentVariable("S3_ACCOUNT_ID") ?? "<ACCOUNT_ID>";
    var credentials = new BasicAWSCredentials(accessKey, secretKey);
    _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
    {
      ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
    });
  }

  public async Task<string?> GetObject(string bucket, string key)
  {
    var s3Object = await _s3Client.GetObjectAsync(bucket, key);
    StreamReader streamReader = new StreamReader(s3Object.ResponseStream);
    var content = await streamReader.ReadToEndAsync();

    if (content == null)
    {
      Console.WriteLine("Conteudo é nulo nessa porra");
      return null;
    }

    Console.WriteLine($"Conteudo aqui para tu porra: {content}");
    return content;
  }
}