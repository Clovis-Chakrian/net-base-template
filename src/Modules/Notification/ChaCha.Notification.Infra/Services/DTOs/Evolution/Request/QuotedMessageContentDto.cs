using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Request;

public class QuotedMessageContentDto
{
    [JsonPropertyName("conversation")]
    public string? Conversation { get; set; }
}