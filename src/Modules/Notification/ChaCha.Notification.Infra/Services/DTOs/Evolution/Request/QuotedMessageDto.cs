using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Request;

public class QuotedMessageDto
{
    [JsonPropertyName("key")]
    public QuotedMessageKeyDto? Key { get; set; }

    [JsonPropertyName("message")]
    public QuotedMessageContentDto? Message { get; set; }
}