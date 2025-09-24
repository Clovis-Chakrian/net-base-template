using ChaCha.MediatR.Mediator;
using ChaCha.Security.Application.Auth.v1.Commands.ConnectToken;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Api.Controllers;

[ApiController]
[Route("~/")]
public class AuthorizationController : ControllerBase
{
  private readonly IMediator _mediator;

  public AuthorizationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost("connect/token"), Produces("application/json")]
  public async Task<IActionResult> Exchange()
  {
    var request = HttpContext.GetOpenIddictServerRequest();
    var result = await _mediator.Send(new ConnectTokenCommand(request!));
    
    if (!result.IsValid)
    {
      return BadRequest(result.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }

    return result.Data!;
  }
}