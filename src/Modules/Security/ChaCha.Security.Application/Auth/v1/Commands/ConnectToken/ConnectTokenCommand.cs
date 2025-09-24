using ChaCha.MediatR.Commands;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Application.Auth.v1.Commands.ConnectToken;

public record ConnectTokenCommand(OpenIddictRequest OpenIddictRequest) : ICommand<SignInResult>;