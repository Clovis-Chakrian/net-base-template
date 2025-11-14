using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace ChaCha.WebApi.Response;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public IEnumerable<ApiError>? Errors { get; init; }
    public ApiMeta? Meta { get; init; }

    public static ApiResponse<T> Ok(
        T? data,
        ApiMeta? meta = null
    ) => new()
    {
        Success = true,
        Data = data,
        Errors = Array.Empty<ApiError>(),
        Meta = meta ?? ApiMeta.Empty()
    };

    public static ApiResponse<T> Fail(
        ValidationResult vr,
        ApiMeta? meta = null
    ) => new()
    {
        Success = false,
        Data = default,
        Errors = vr.Errors.Select(ApiError.FromValidationFailure),
        Meta = meta ?? ApiMeta.Empty()
    };

    public static ApiResponse<T> Fail(
        IEnumerable<ApiError> errors,
        ApiMeta? meta = null
    ) => new()
    {
        Success = false,
        Data = default,
        Errors = errors,
        Meta = meta ?? ApiMeta.Empty()
    };
}

public sealed record ApiError(
    string Message,
    string? Code = null,
    string? Field = null,
    string? Help = null)
{
    public static ApiError FromValidationFailure(ValidationFailure f) =>
        new(
            Message: f.ErrorMessage,
            Code: string.IsNullOrWhiteSpace(f.ErrorCode) ? null : f.ErrorCode,
            Field: string.IsNullOrWhiteSpace(f.PropertyName) ? null : f.PropertyName
        );
}

public sealed record ApiMeta(
    string TraceId,
    DateTimeOffset Timestamp,
    string Path,
    string Method,
    object? Extra = null)
{
    public static ApiMeta Empty() =>
        new(
            TraceId: string.Empty,
            Timestamp: DateTimeOffset.UtcNow,
            Path: string.Empty,
            Method: string.Empty,
            Extra: null
        );
}