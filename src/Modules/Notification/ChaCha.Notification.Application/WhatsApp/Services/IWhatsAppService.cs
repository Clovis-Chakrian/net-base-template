using ChaCha.MediatR;

namespace ChaCha.Notification.Application.WhatsApp.Services;

public interface IWhatsAppService
{
    Task<Result<string>> SendTextAsync(string instance, string sendTo, string message);
}