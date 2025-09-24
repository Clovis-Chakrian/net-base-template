using ChaCha.MediatR.Mediator;
using ChaCha.Notification.Application.NotificationsSent.Commands.NewUserCreated;
using Microsoft.AspNetCore.Mvc;

namespace ChaCha.Notification.Api.Controllers.v1;

[ApiController]
[Route("api/v1/notifications")]
public class NotificationController
{
  private readonly IMediator _mediator;

  public NotificationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] NewUserCreatedCommand command)
  {
    var result = await _mediator.Send(command);

    if (!result.IsValid)
    {
      return new BadRequestObjectResult(result.ValidationResult);
    }

    return new OkObjectResult(result.ValidationResult);
  }
}