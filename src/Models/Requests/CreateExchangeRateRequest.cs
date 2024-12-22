using CurrencyExchange.Api.Validation;

namespace CurrencyExchange.Api.Models.Requests;

/// <summary>
/// Exchange rate data form.
/// </summary>
public class CreateExchangeRateRequest {
    /// <summary>
    /// Base currency code.
    /// </summary>
    [ValidCurrencyCode]
    public required string BaseCurrencyCode { get; init; }

    /// <summary>
    /// Target currency code.
    /// </summary>
    [ValidCurrencyCode]
    public required string TargetCurrencyCode { get; init; }

    /// <summary>
    /// Exchange rate.
    /// </summary>
    [GreaterThanZero]
    public required double Rate { get; init; }
}