namespace ChaCha.Bus;

public class TesteEvent : IntegrationEvent
{
  public new static string QueueName => "teste-queue";
}