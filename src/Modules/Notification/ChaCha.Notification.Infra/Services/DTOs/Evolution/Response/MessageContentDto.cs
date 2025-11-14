using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Response;

public class MessageContentDto
{
    [JsonPropertyName("extendedTextMessage")]
    public ExtendedTextMessageDto? ExtendedTextMessage { get; set; }
}