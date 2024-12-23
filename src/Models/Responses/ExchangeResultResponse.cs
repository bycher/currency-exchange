namespace CurrencyExchange.Api.Models.Responses;

/// <summary>
/// Response with exchange result data.
/// </summary>
public class ExchangeResultResponse {
    /// <summary>
    /// Base currency response data.
    /// </summary>
    public required CurrencyResponse BaseCurrency { get; init; }

    /// <summary>
    /// Target currency response data.
    /// </summary>
    public required CurrencyResponse TargetCurrency { get; init; }

    /// <summary>
    /// Exchange rate.
    /// </summary>
    public required decimal Rate { get; init; }

    /// <summary>
    /// Amount to exchange.
    /// </summary>
    public required decimal Amount { get; init; }

    /// <summary>
    /// Converted amount by exchange rate.
    /// </summary>
    public decimal ConvertedAmount => Rate * Amount;
}