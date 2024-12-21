namespace CurrencyExchange.Api.Models.Responses;

/// <summary>
/// Response with exchange rate data.
/// </summary>
public class ExchangeRateResponse {
    /// <summary>
    /// Exchange rate ID.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Base currency response data.
    /// </summary>
    public required CurrencyResponse BaseCurrency { get; set; }

    /// <summary>
    /// Target currency response data.
    /// </summary>
    public required CurrencyResponse TargetCurrency { get; set; }

    /// <summary>
    /// Exchange rate.
    /// </summary>
    public required double Rate { get; init; }
}