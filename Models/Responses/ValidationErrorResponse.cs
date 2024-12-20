namespace CurrencyExchange.Models.Responses;

/// <summary>
/// Response for validation errors.
/// </summary>
/// <param name="errors">Validation errors.</param>
public class ValidationErrorResponse(Dictionary<string, string[]> errors)
    : ErrorResponse(400, "Validation is failed") {
    /// <summary>
    /// Validation errors.
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; } = errors;
}
