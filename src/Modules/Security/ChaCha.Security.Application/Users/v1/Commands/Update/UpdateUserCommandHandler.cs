using ChaCha.MediatR;
using ChaCha.MediatR.Commands;

namespace ChaCha.Security.Application.Users.v1.Commands.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdatedUserDto>
{
  public Task<Result<UpdatedUserDto>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
  {
    var result = Result<UpdatedUserDto>.Create();
    // Implement your user update logic here
    var updatedUser = new UpdatedUserDto("User updated successfully");
    return Task.FromResult(result.Success(updatedUser));
  }
}

// Result<UpdatedUserDto>.Success(updatedUser)