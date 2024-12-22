using CurrencyExchange.Api.Validation;

namespace CurrencyExchange.Api.Models.Requests;

/// <summary>
/// Currency data form.
/// </summary>
public class CreateCurrencyRequest {
    /// <summary>
    /// Currency code.
    /// </summary>
    [ValidCurrencyCode]
    public required string Code { get; init; }

    /// <summary>
    /// Currency name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Currency sign.
    /// </summary>
    public required string Sign { get; init; }
}