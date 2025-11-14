using ChaCha.Data.Persistence.UnitOfWork;
using ChaCha.MediatR;
using ChaCha.MediatR.Commands;
using ChaCha.Notification.Application.WhatsApp.Services;
using ChaCha.Notification.Domain.NotificationChannels;
using ChaCha.Notification.Domain.NotificationExternalProviders;
using ChaCha.Notification.Domain.NotificationsSent;
using ChaCha.Notification.Domain.NotificationsSent.Repositories;
using ChaCha.Notification.Domain.NotificationTemplates.Repositories;
using ChaCha.Notification.Domain.NotificationTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.WhatsApp.Commands.Send;

public class SendWhatsAppCommandHandler : ICommandHandler<SendWhatsAppCommand>
{
    private readonly IWhatsAppService _whatsAppService;
    private readonly ILogger<SendWhatsAppCommandHandler> _logger;
    private readonly INotificationSentWriteRepository _notificationSentWriteRepository;
    private readonly INotificationTemplateReadRepository _notificationTemplateReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendWhatsAppCommandHandler(
        IWhatsAppService whatsAppService,
        ILogger<SendWhatsAppCommandHandler> logger,
        INotificationSentWriteRepository notificationSentWriteRepository,
        INotificationTemplateReadRepository notificationTemplateReadRepository,
        IUnitOfWork unitOfWork
        )
    {
        _whatsAppService = whatsAppService;
        _logger = logger;
        _notificationSentWriteRepository = notificationSentWriteRepository;
        _notificationTemplateReadRepository = notificationTemplateReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SendWhatsAppCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando o envio de whatsapp com o template recebido: {TemplateId}. Enviando pela instância {Instance}", command.TemplateId, command.Instance);
        var result = Result.Create();

        var notificationTemplate = await _notificationTemplateReadRepository.FindAsync(p => p.Id == command.TemplateId, ["NotificationTemplateParams"], useCache: false);

        if (notificationTemplate is null)
        {
            _logger.LogError("Não foi possível realizar o envio da mensagem via whatsapp porque não foi possível encontrar o template selecionado. Template id recebido: {TemplateId}", command.TemplateId);

            result.AddFailure($"Não foi possível encontrar um template para o id {command.TemplateId} recebido no comando.");

            return result;
        }

        if ((ENotificationChannel)notificationTemplate.NotificationChannelId != ENotificationChannel.WhatsApp)
        {
            _logger.LogError("Não foi possível realizar o envio da mensagem via whatsapp porque o template selecionado não é pertencente ao canal whatsapp. Template id recebido: {TemplateId}", command.TemplateId);

            result.AddFailure($"Não foi possível realizar o envio da mensagem via whatsapp porque o template selecionado não é pertencente ao canal whatsapp. Template id recebido no comando: {command.TemplateId}");

            return result;
        }

        var notificationSentResult = NotificationSent.Create(
            notificationTemplateId: notificationTemplate.Id,
            notificationChannel: ENotificationChannel.WhatsApp,
            notificationExternalProvider: ENotificationExternalProvider.Evolution,
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
        var formatedMessage = notificationTemplate.Body;

        if (command.TemplateParams is not null)
        {
            foreach (var param in notificationTemplate.NotificationTemplateParams)
            {
                formatedMessage = formatedMessage?.Replace($"{{{param.Name}}}", command.TemplateParams[param.Name]);
                notificationSent.AddParam(param.Id, command.TemplateParams[param.Name]);
            }
        }

        var sendWhatsAppResult = await _whatsAppService.SendTextAsync(
            instance: command.Instance,
            sendTo: command.To,
            message: formatedMessage!
            );

        if (!sendWhatsAppResult.IsValid)
        {
            _logger.LogError("Não foi possível enviar o whatsapp solicitado. NotificationSentId: {NotificationSentId} - Errors: {Errors}", notificationSent.Id, sendWhatsAppResult.ValidationResult.Errors);
            notificationSent.AddReasonNotSent(sendWhatsAppResult.ValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage!);
            result.HandleResult(sendWhatsAppResult.ValidationResult!);
        }

        await _notificationSentWriteRepository.CreateAsync(notificationSent);
        await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

        return result;
    }
}