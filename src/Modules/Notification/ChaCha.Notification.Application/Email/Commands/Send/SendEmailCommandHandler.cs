using ChaCha.Data.Persistence.UnitOfWork;
using ChaCha.MediatR;
using ChaCha.MediatR.Commands;
using ChaCha.Notification.Application.Email.Services;
using ChaCha.Notification.Domain.NotificationChannels;
using ChaCha.Notification.Domain.NotificationExternalProviders;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationsSent.Repositories;
using ChaCha.Notification.Domain.NotificationTemplates.Repositories;
using ChaCha.Notification.Domain.NotificationTypes;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.Email.Commands.Send;

public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
{
    private readonly ILogger<SendEmailCommandHandler> _logger;
    private readonly INotificationSentWriteRepository _notificationSentWriteRepository;
    private readonly INotificationTemplateReadRepository _notificationTemplateReadRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public SendEmailCommandHandler(
        ILogger<SendEmailCommandHandler> logger,
        INotificationSentWriteRepository notificationSentWriteRepository,
        INotificationTemplateReadRepository notificationTemplateReadRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _notificationSentWriteRepository = notificationSentWriteRepository;
        _notificationTemplateReadRepository = notificationTemplateReadRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SendEmailCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando o envio de email para o id de template recebido: {TemplateId}. Enviando para {Tos} e com cópia para {Ccs}.", command.TemplateId, command.Tos, command.Ccs);
        var result = Result.Create();

        var notificationTemplate = await _notificationTemplateReadRepository.FindAsync(p => p.Id == command.TemplateId, ["NotificationTemplateParams"], useCache: false);

        if (notificationTemplate is null)
        {
            _logger.LogError("Não foi possível realizar o envio do email porque não foi possível encontrar o template selecionado. Template id recebido: {TemplateId}", command.TemplateId);

            result.AddFailure($"Não foi possível encontrar um template para o id {command.TemplateId} recebido no comando.");

            return result;
        }

        var notificationSentResult = NotificationSent.Create(
            notificationTemplateId: notificationTemplate.Id,
            notificationChannel: ENotificationChannel.Email,
            notificationExternalProvider: ENotificationExternalProvider.Mailgun,
            notificationType: ENotificationType.Trasactional,
            shouldRetry: true,
            userId: Guid.NewGuid(),
            correlationId: Guid.NewGuid()
        );

        if (!notificationSentResult.IsValid || notificationSentResult.Data is null)
        {
            _logger.LogError("Não foi possível criar o NotificationSent pelo seguinte erro: {Errors}", notificationSentResult.ValidationResult.Errors);
            result.HandleResult(notificationSentResult.ValidationResult);

            return result;
        }

        var notificationSent = notificationSentResult.Data;

        if (command.TemplateParams is not null)
        {
            foreach (var param in notificationTemplate.NotificationTemplateParams)
            {
                notificationSent.AddParam(param.Id, command.TemplateParams[param.Name]);
            }
        }

        var sendEmailResult = await _emailService.SendEmail(
            templateId: notificationTemplate.ExternalTemplateId!,
            tos: command.Tos,
            subject: command.Subject,
            templateParams: command.TemplateParams
            );

        if (!sendEmailResult.IsValid)
        {
            _logger.LogError("Não foi possível enviar o email solicitado. NotificationSentId: {NotificationSentId} - Errors: {Errors}", notificationSent.Id, sendEmailResult.ValidationResult.Errors);
            notificationSent.AddReasonNotSent(sendEmailResult.ValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage!);
            result.HandleResult(sendEmailResult.ValidationResult!);
        }

        await _notificationSentWriteRepository.CreateAsync(notificationSent);
        await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

        return result;
    }
}
