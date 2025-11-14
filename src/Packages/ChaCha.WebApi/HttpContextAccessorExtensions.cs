using ChaCha.WebApi.Response;
using Microsoft.AspNetCore.Http;

namespace ChaCha.WebApi;

public static class HttpContextAccessorExtensions
{
    public static bool IsGet(this HttpContext ctx) => HttpMethods.IsGet(ctx.Request.Method);
    public static bool IsPost(this HttpContext ctx) => HttpMethods.IsPost(ctx.Request.Method);
    public static bool IsPutOrPatch(this HttpContext ctx) =>
        HttpMethods.IsPut(ctx.Request.Method) || HttpMethods.IsPatch(ctx.Request.Method);
    public static bool IsDelete(this HttpContext ctx) => HttpMethods.IsDelete(ctx.Request.Method);

    public static ApiMeta BuildMeta(this HttpContext ctx, object? extra = null) =>
        new(
            TraceId: ctx.TraceIdentifier,
            Timestamp: DateTimeOffset.UtcNow,
            Path: ctx.Request.Path.HasValue ? ctx.Request.Path.Value! : "/",
            Method: ctx.Request.Method,
            Extra: extra
        );
}