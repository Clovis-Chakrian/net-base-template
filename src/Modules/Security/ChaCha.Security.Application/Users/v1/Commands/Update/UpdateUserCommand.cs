using ChaCha.MediatR.Commands;

namespace ChaCha.Security.Application.Users.v1.Commands.Update;

public record UpdateUserCommand() : ICommand<UpdatedUserDto>;