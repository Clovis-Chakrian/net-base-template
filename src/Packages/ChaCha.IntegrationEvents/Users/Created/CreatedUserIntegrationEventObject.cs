namespace ChaCha.IntegrationEvents.Users.Created;

public class CreatedUserIntegrationEventObject
{
  public Guid UserId { get; private set; }
  public string Email { get; private set; }
  public string FullName { get; private set; }

  public CreatedUserIntegrationEventObject(Guid userId, string email, string fullName)
  {
    UserId = userId;
    Email = email;
    FullName = fullName;
  }
}