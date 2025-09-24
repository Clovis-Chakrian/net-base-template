using ChaCha.MediatR.Commands;

namespace ChaCha.Security.Application.Users.v1.Commands.Create;

public record CreateUserCommand(string FullName, string Email, string Password) : ICommand;