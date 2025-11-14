namespace ChaCha.IntegrationEvents.Notifications.Emails;

public class SendEmailIntegrationEventObject
{
    public Guid TemplateId { get; private set; }
    public string Subject { get; private set; }
    public IEnumerable<string> Tos { get; set; }
    public IEnumerable<string>? Ccs { get; set; }
    public IEnumerable<string>? Bccs { get; set; }
    public Dictionary<string, string>? TemplateParams { get; set; }

    public SendEmailIntegrationEventObject(
        Guid templateId, 
        string subject,
        IEnumerable<string> tos,
        IEnumerable<string>? ccs = null,
        IEnumerable<string>? bccs = null,
        Dictionary<string, string>? templateParams = null
        )
    {
        TemplateId = templateId;
        Subject = subject;
        Tos = tos;
        Ccs = ccs;
        Bccs = bccs;
        TemplateParams = templateParams;
    }
}