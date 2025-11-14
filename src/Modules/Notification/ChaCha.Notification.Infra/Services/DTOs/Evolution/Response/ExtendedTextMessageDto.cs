using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Response;

public class ExtendedTextMessageDto
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}