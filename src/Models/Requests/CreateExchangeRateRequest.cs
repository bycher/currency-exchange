using CurrencyExchange.Api.Validation;

namespace CurrencyExchange.Api.Models.Requests;

/// <summary>
/// Exchange rate data form.
/// </summary>
public class CreateExchangeRateRequest(
    string baseCurrencyCode, string targetCurrencyCode, double rate
) {
    /// <summary>
    /// Base currency code.
    /// </summary>
    [ValidCurrencyCode]
    public string BaseCurrencyCode { get; init; } = baseCurrencyCode;

    /// <summary>
    /// Target currency code.
    /// </summary>
    [ValidCurrencyCode]
    public string TargetCurrencyCode { get; init; } = targetCurrencyCode;

    /// <summary>
    /// Exchange rate.
    /// </summary>
    [GreaterThanZero]
    public double Rate { get; init; } = rate;
}