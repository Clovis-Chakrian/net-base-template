using ChaCha.MediatR.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ChaCha.Security.Application.Auth.v1.Queries.UserInfo;

public record UserInfoQuery() : IQuery<IActionResult>;
