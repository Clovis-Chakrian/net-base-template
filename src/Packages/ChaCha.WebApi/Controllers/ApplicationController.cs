using ChaCha.MediatR;
using ChaCha.WebApi.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChaCha.WebApi.Controllers;

[ApiController]
public abstract class ApplicationController : ControllerBase
{
    private readonly IHttpContextAccessor _http;

    protected ApplicationController(IHttpContextAccessor httpContextAccessor)
    {
        _http = httpContextAccessor;
    }

    /// <summary>
    /// Responde um Result{T} com envelope ApiResponse{T} e status code adequado ao método HTTP.
    /// POST -> 201 (com Location) ou 200; PUT/PATCH -> 200 ou 204; DELETE -> 204; GET -> 200/404.
    /// Validação -> 422 Unprocessable Entity.
    /// </summary>
    /// <typeparam name="T">Tipo do payload de dados.</typeparam>
    /// <param name="result">Resultado do caso de uso.</param>
    /// <param name="locationFactory">
    /// Opcional: função para construir a URL do recurso criado (usado em POST para 201 Created).
    /// </param>
    /// <param name="extraMeta">Objeto livre para enriquecer o Meta.Extra (correlationId, flags, etc.).</param>
    protected ActionResult<ApiResponse<T>> Respond<T>(
        Result<T> result,
        Func<T, string?>? locationFactory = null,
        object? extraMeta = null)
    {
        var ctx = _http.HttpContext ?? HttpContext;
        var meta = BuildMeta(ctx, extraMeta);

        if (!result.IsValid)
        {
            var body = ApiResponse<T>.Fail(result.ValidationResult, meta);
            return UnprocessableEntity(body);
        }

        if (ctx.IsPost())
        {
            if (EqualityComparer<T>.Default.Equals(result.Data, default!))
            {
                return NoContent();
            }

            var location = locationFactory?.Invoke(result.Data!);
            var body = ApiResponse<T>.Ok(result.Data, meta);

            if (!string.IsNullOrWhiteSpace(location))
                return Created(location!, body);

            return Ok(body);
        }

        if (ctx.IsPutOrPatch())
        {
            if (!EqualityComparer<T>.Default.Equals(result.Data, default!))
            {
                return Ok(ApiResponse<T>.Ok(result.Data, meta));
            }
            return NoContent();
        }

        if (ctx.IsDelete())
        {
            return NoContent();
        }

        if (ctx.IsGet())
        {
            if (IsEnumerable(typeof(T)))
            {
                return Ok(ApiResponse<T>.Ok(result.Data, meta));
            }

            if (EqualityComparer<T>.Default.Equals(result.Data, default!))
            {
                var notFoundErrors = new[]
                {
                    new ApiError("Resource not found", Code: "NOT_FOUND")
                };
                return NotFound(ApiResponse<T>.Fail(notFoundErrors, meta));
            }

            return Ok(ApiResponse<T>.Ok(result.Data, meta));
        }

        return Ok(ApiResponse<T>.Ok(result.Data, meta));
    }

    /// <summary>
    /// Responde um Result (sem payload) com status code adequado ao método HTTP.
    /// POST/PUT/PATCH/DELETE -> 204 NoContent; GET -> 204; Validação -> 422.
    /// </summary>
    protected ActionResult<ApiResponse<object>> Respond(Result result, object? extraMeta = null)
    {
        var ctx = _http.HttpContext ?? HttpContext;
        var meta = BuildMeta(ctx, extraMeta);

        if (!result.IsValid)
        {
            return UnprocessableEntity(ApiResponse<object>.Fail(result.ValidationResult, meta));
        }

        if (ctx.IsPost() || ctx.IsPutOrPatch() || ctx.IsDelete())
            return NoContent();

        return NoContent();
    }

    private static bool IsEnumerable(Type t)
    {
        if (t == typeof(string)) return false;
        return typeof(System.Collections.IEnumerable).IsAssignableFrom(t);
    }

    private static ApiMeta BuildMeta(HttpContext ctx, object? extra = null) =>
        new(
            TraceId: ctx.TraceIdentifier,
            Timestamp: DateTimeOffset.UtcNow,
            Path: ctx.Request.Path.HasValue ? ctx.Request.Path.Value! : "/",
            Method: ctx.Request.Method,
            Extra: extra
        );
}