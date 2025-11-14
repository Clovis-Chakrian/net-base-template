using ChaCha.MediatR;

namespace ChaCha.Notification.Application.Email.Services;

public interface IEmailService
{
  Task<Result<string>> SendEmail(
    string templateId,
    IEnumerable<string> tos,
    string? subject = null,
    Dictionary<string, string>? templateParams = null);
}