namespace ChaCha.Bus;

public abstract class IntegrationEvent
{
  public static string QueueName => "";
  public static string Topic => "";
  public static string Exchange => "";
}