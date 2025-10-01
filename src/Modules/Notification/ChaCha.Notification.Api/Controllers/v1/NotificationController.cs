using ChaCha.Core.Repositories.Pagination;
using ChaCha.MediatR.Mediator;
using ChaCha.Notification.Application.TokenTypes.Queries.FindAll;
using ChaCha.Notification.Domain.TokenTypes;
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

  [HttpGet("token-types")]
  public async Task<IActionResult> Get()
  {
    return new OkObjectResult(await _mediator.Send(new FindAllTokenTypesQuery()));
  }
}