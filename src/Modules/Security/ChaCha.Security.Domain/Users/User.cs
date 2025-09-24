using ChaCha.Auth.Persistence;
using ChaCha.MediatR;
using ChaCha.Security.Domain.Users.Events;
using Microsoft.AspNetCore.Identity;

namespace ChaCha.Security.Domain.Users;

public class User : ApplicationUser
{
  public string? FullName { get; private set; }

  private User() { }

  private User(string fullName, string email, string password) : base()
  {
    PasswordHasher<User> passwordHasher = new();

    FullName = fullName;
    Email = email;
    UserName = email;
    PasswordHash = passwordHasher.HashPassword(this, password);
    this.AddDomainEvent(new UserCreatedEvent(Email: Email, UserId: Id, FullName: FullName));
  }

  public static Result<User> Create(string fullName, string email, string password)
  {
    var result = Result<User>.Create();
    
    return result.Success(new User(fullName, email, password));
  }
}