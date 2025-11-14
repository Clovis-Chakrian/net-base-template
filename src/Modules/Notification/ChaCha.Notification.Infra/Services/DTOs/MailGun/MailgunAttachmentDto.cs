namespace ChaCha.Notification.Infra.Services.DTOs.MailGun;

public class MailgunAttachmentDto
{
    public string FileName { get; init; } = default!;
    public byte[] Content { get; init; } = default!;
    public string? ContentType { get; init; }
    public string? ContentId { get; init; }
}