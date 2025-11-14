using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Response;

public class MessageKeyDto
{
    [JsonPropertyName("remoteJid")]
    public string? RemoteJid { get; set; }

    [JsonPropertyName("fromMe")]
    public bool FromMe { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}