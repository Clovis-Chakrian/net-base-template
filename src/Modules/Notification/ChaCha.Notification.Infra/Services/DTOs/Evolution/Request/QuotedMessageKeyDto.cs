using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Request;

public class QuotedMessageKeyDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}