using ChaCha.Notification.Application.Email.Services;
using ChaCha.Notification.Infra.Services.DTOs.MailGun;
using Microsoft.Extensions.Logging;
using ChaCha.MediatR;
using System.Text;

namespace ChaCha.Notification.Infra.Services;

public class EmailService : IEmailService
{
  private readonly HttpClient _httpClient;
  private readonly ILogger<EmailService> _logger;

  public EmailService(ILogger<EmailService> logger)
  {
    _httpClient = new HttpClient();
    var emailApiBaseUrl = Environment.GetEnvironmentVariable("EMAIL_API_BASE_URL") ?? "";
    _httpClient.BaseAddress = new Uri(emailApiBaseUrl);
    var user = Environment.GetEnvironmentVariable("EMAIL_USER");
    var key = Environment.GetEnvironmentVariable("EMAIL_API_KEY");
    var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{key}"));
    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
    _logger = logger;
  }

  public async Task<Result<string>> SendEmail(
    string templateId,
    IEnumerable<string> tos,
    string? subject = null,
    Dictionary<string, string>? templateParams = null
    )
  {
    _logger.LogInformation("Iniciando disparo de envio de email utilizando o template: {TemplateId}.", templateId);
    var result = Result<string>.Create();

    var builder = new MailGunMessageRequestDto.Builder()
      .From("Modular Monolith <postmaster@sandbox2c4acdbfaab24ebb9cd9bb180cac641e.mailgun.org>")
      .To([.. tos])
      .Subject(subject ?? "")
      .UseTemplate(templateId)
      .Header("Reply-To", "clovischakriandev@gmail.com")
      .Delivery(new MailgunDeliveryOptionsDto
      {
        Tracking = EMailgunTriState.Yes,
        RequireTls = EMailgunTriState.True
      });

    if (templateParams is not null)
    {
      _logger.LogInformation("A propriedade templateParams não é nula, iniciando o preenchimento das variáveis do tempalte.");

      foreach (var param in templateParams)
      {
        builder.TemplateVar(param.Key, param.Value);
      }
    }

    var request = builder.Build();

    try
    {
      using var content = request.ToMultipartFormData();
      var response = await _httpClient.PostAsync("v3/sandbox2c4acdbfaab24ebb9cd9bb180cac641e.mailgun.org/messages", content);
      var responseFormated = await MailgunMessageResponseDto.FromHttpResponseAsync(response);

      if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
      {
        _logger.LogError("Não foi possível enviar o email. O serviço de email retornou status code 401 ao tentar enviar o email.");

        result.AddFailure("O serviço de envio de email retornou status code 401 (Forbidden) - Recomendo verificar as credenciais de acesso para a ferramenta de envio de email utilizada.");

        return result;
      }

      if (!response.IsSuccessStatusCode)
      {
        var stringResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError("Não foi possível enviar o email. O serviço de envio de email retornou a seguinte resposta: {Response}. O retorno completo foi esse: {HttpResponse}", responseFormated.Message,stringResponse);

        result.AddFailure(responseFormated.Message!);

        return result;
      }

      _logger.LogInformation("Email enfileirado com sucesso. O webhook deve retornar uma resposta quando o email for entregue.");

      return result.Success(responseFormated.Id!);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Não foi possível enviar o email, ocorreu uma exceção ao tentar enviar. Mensagem da exceção: {ExceptionMessage} | Exceção: {Exception}", ex.Message, ex);

      result.AddFailure(ex.Message);

      return result;
    }
  }
}