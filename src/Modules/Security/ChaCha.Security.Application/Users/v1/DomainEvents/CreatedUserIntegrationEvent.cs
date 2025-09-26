using ChaCha.Bus;
using EasyNetQ;

namespace ChaCha.Security.Application.Users.v1.DomainEvents;

public class CreatedUserIntegrationEvent : IntegrationEvent<Message<string>, string>
{
  public CreatedUserIntegrationEvent(string userEmail) : base(new Message<string>(userEmail), "user.events", "user.created")
  {
  }
}
