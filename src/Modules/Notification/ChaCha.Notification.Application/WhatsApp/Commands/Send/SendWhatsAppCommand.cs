using ChaCha.MediatR.Commands;

namespace ChaCha.Notification.Application.WhatsApp.Commands.Send;

public record SendWhatsAppCommand(
    Guid TemplateId, 
    string To,
    string Instance,
    string? Subject = null, 
    Dictionary<string, string>? TemplateParams = null
) : ICommand;