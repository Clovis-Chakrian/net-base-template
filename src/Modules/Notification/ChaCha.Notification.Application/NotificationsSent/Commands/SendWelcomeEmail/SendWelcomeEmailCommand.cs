using ChaCha.MediatR.Commands;

namespace ChaCha.Notification.Application.NotificationsSent.Commands.SendWelcomeEmail;

public record SendWelcomeEmailCommand(
    string Email,
    string FullName) : ICommand;