using ChaCha.MediatR.Commands;

namespace ChaCha.Notification.Application.NotificationsSent.Commands.NewUserCreated;

public record NewUserCreatedCommand(
    string Email,
    string FullName) : ICommand;