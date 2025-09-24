using System.Threading.Tasks;
using ChaCha.MediatR.Mediator;
using ChaCha.Security.Application.Users.v1.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace ChaCha.Security.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController
{
  private readonly IMediator _mediator;

  public UserController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
  {
    var result = await _mediator.Send(command);

    if (!result.IsValid)
    {
      return new BadRequestObjectResult(result.ValidationResult);
    }

    return new OkObjectResult(result.ValidationResult);
  }
}