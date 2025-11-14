using System.Net;
using System.Text.Json;

namespace ChaCha.Notification.Infra.Services.DTOs.MailGun;

public class MailgunMessageResponseDto
{
    /// <summary>HTTP status code da requisição.</summary>
    public HttpStatusCode StatusCode { get; init; }

    /// <summary>Indica se a requisição foi sucesso (2xx).</summary>
    public bool IsSuccess { get; init; }

    /// <summary>Mensagem retornada pela API (ex.: "Queued. Thank you.").</summary>
    public string? Message { get; init; }

    /// <summary>ID do enfileiramento no Mailgun (ex.: &lt;2025...@dominio&gt;).</summary>
    public string? Id { get; init; }

    /// <summary>Corpo bruto retornado pela API.</summary>
    public string RawBody { get; init; } = string.Empty;

    /// <summary>Headers de resposta úteis para auditoria/debug.</summary>
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; init; } = new Dictionary<string, IEnumerable<string>>();

    public static async Task<MailgunMessageResponseDto> FromHttpResponseAsync(HttpResponseMessage http)
    {
        var raw = await http.Content.ReadAsStringAsync().ConfigureAwait(false);

        string? id = null, message = null;
        try
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            if (root.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.String)
                id = idProp.GetString();

            if (root.TryGetProperty("message", out var msgProp) && msgProp.ValueKind == JsonValueKind.String)
                message = msgProp.GetString();
        }
        catch
        {
            // se a resposta não for JSON, mantém somente RawBody
        }

        return new MailgunMessageResponseDto
        {
            StatusCode = http.StatusCode,
            IsSuccess = http.IsSuccessStatusCode,
            Id = id,
            Message = message,
            RawBody = raw,
            Headers = http.Headers.Concat(http.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
                                .ToDictionary(h => h.Key, h => h.Value)
        };
    }
}