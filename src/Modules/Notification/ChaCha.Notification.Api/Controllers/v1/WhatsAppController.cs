using ChaCha.Notification.Application.WhatsApp.Services;
using Microsoft.AspNetCore.Authorization;
using ChaCha.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChaCha.MediatR.Mediator;

namespace ChaCha.Notification.Api.Controllers.v1;

[Authorize]
[Route("api/v1/notifications")]
public class NotificationController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly IWhatsAppService _whatsAppService;

    public NotificationController(IMediator mediator, IHttpContextAccessor httpContext, IWhatsAppService whatsAppService) : base(httpContext)
    {
        _mediator = mediator;
        _whatsAppService = whatsAppService;
    }
}