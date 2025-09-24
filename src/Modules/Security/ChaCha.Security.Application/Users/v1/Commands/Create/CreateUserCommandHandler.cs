using ChaCha.Security.Domain.Users;
using ChaCha.MediatR;
using ChaCha.MediatR.Commands;
using Microsoft.AspNetCore.Identity;

namespace ChaCha.Security.Application.Users.v1.Commands.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
  private readonly UserManager<User> _userManager;
  public CreateUserCommandHandler(UserManager<User> userManager)
  {
    _userManager = userManager;
  }

  public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    var result = Result.Create();
    var createUserResult = User.Create(email: command.Email, fullName: command.FullName, password: command.Password);

    if (!createUserResult.IsValid)
    {
      result.AddFailure(createUserResult.ValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage!);
      return result;
    }

    var identityResult = await _userManager.CreateAsync(createUserResult.Data!);

    if (!identityResult.Succeeded)
    {
      foreach (var err in identityResult.Errors)
      {
        result.AddFailure(err.Description);
      }

      return result;
    }

    return await Task.FromResult(result.Success());
  }
}