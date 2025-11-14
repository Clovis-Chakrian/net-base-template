using ChaCha.MediatR.Commands;

namespace ChaCha.Notification.Application.Email.Commands.Send;

public record SendEmailCommand(
    Guid TemplateId, 
    string Subject, 
    IEnumerable<string> Tos, 
    IEnumerable<string>? Ccs = null, 
    IEnumerable<string>? Bccs = null, 
    Dictionary<string, string>? TemplateParams = null
    ) : ICommand;
