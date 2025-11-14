using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Response;

public class SendTextMessageResponseDto
{
    [JsonPropertyName("key")]
    public MessageKeyDto? Key { get; set; }

    [JsonPropertyName("message")]
    public MessageContentDto? Message { get; set; }

    [JsonPropertyName("messageTimestamp")]
    public int? MessageTimestamp { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}