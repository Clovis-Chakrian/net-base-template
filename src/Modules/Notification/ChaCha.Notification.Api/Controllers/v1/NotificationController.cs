// using ChaCha.Core.Repositories.Pagination;
// using ChaCha.MediatR.Mediator;
// using ChaCha.Notification.Application.Email;
// using ChaCha.WebApi.Controllers;
// using ChaCha.WebApi.Response;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace ChaCha.Notification.Api.Controllers.v1;

// [Authorize]
// [Route("api/v1/notifications")]
// public class NotificationController : ApplicationController
// {
//   private readonly IMediator _mediator;

//   public NotificationController(IMediator mediator, IHttpContextAccessor httpContext, IEmailService emailService) : base(httpContext)
//   {
//     _mediator = mediator;
//     _emailService = emailService;
//   }

//   [HttpGet("token-types")]
//   public async Task<ActionResult<ApiResponse<Page<TokenTypeListDto>>>> Get()
//   {
//     return Respond(await _mediator.Send(new FindAllTokenTypesQuery()));
//   }

//   [AllowAnonymous]
//   [HttpGet("teste")]
//   public async Task<ActionResult> GetTeste()
//   {
//     return new OkObjectResult(_emailService.SendEmail());
//   }
// }