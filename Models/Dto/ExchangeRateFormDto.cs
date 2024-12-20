using CurrencyExchange.Validation;

namespace CurrencyExchange.Models.Dto;

/// <summary>
/// Exchange rate data form.
/// </summary>
/// <param name="BaseCurrencyCode">Base currency code.</param>
/// <param name="TargetCurrencyCode">Target currency code.</param>
/// <param name="Rate">Exchange rate.</param>
public record ExchangeRateFormDto(
    [ValidCurrencyCode] string BaseCurrencyCode,
    [ValidCurrencyCode] string TargetCurrencyCode,
    [GreaterThanZero] double Rate
);