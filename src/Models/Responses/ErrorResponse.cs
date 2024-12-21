using System.Text.Json.Serialization;

namespace CurrencyExchange.Api.Models.Responses;

/// <summary>
/// Error response.
/// </summary>
public class ErrorResponse(int statusCode, string message, string? details = null) {
    /// <summary>
    /// Response status code.
    /// </summary>
    public int StatusCode { get; init; } = statusCode;

    /// <summary>
    /// Response message.
    /// </summary>
    public string Message { get; init; } = message;

    /// <summary>
    /// Details (inner exception message).
    /// Not included in the response JSON if it is null.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Details { get; init; } = details;
}