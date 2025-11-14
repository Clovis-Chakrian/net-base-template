using System.Net.Http.Json;
using System.Text.Json;
using ChaCha.MediatR;
using ChaCha.Notification.Application.WhatsApp.Services;
using ChaCha.Notification.Infra.Services.DTOs.Evolution.Request;
using ChaCha.Notification.Infra.Services.DTOs.Evolution.Response;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Infra.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<WhatsAppService> _logger;

    public WhatsAppService(ILogger<WhatsAppService> logger)
    {
        _httpClient = new HttpClient();
        if (_httpClient.BaseAddress == null)
        {
            var apiBaseUrl = Environment.GetEnvironmentVariable("EVOLUTION_API_BASE_URL");
            _httpClient.BaseAddress = new Uri(apiBaseUrl ?? "");
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("apikey"))
        {
            var apikey = Environment.GetEnvironmentVariable("EVOLUTION_API_KEY");
            _httpClient.DefaultRequestHeaders.Add("apikey", apikey);
        }

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        _logger = logger;
    }

    public async Task<Result<string>> SendTextAsync(string instance, string sendTo, string message)
    {
        _logger.LogInformation("Iniciando envio de whatsapp pela instância {Instance}.", instance);
        var result = Result<string>.Create();

        var request = SendTextMessageRequestDto
            .Builder
            .WithNumber(sendTo)
            .MentionEveryOne(false)
            .WithDelay(100)
            .WithText(message)
            .Build();

        var url = $"message/sendText/{instance}";

        using var response = await _httpClient.PostAsJsonAsync(url, request, _jsonOptions, default);

        var content = await response.Content.ReadAsStringAsync(default);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Erro ao tentar enviar mensagem de whatsapp para a instância {Instance} - Status code: {StatusCode} e o corpo da response {Content}", instance, response.StatusCode, content);

            result.AddFailure($"Erro ao tentar enviar mensagem de whatsapp para a instância {instance} - Status code: {response.StatusCode} e o corpo da response {content}");

            return result;
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            result.AddFailure("Erro no content");
            return result;
        }

        var resultDes = JsonSerializer.Deserialize<SendTextMessageResponseDto>(content, _jsonOptions);
        return result.Success(resultDes?.Key?.Id!);
    }
}