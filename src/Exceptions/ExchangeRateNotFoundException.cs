namespace CurrencyExchange.Api.Exceptions;

/// <summary>
/// Exception thrown when an exchange rate is not found (service layer).
/// Provides user-friendly message.
/// </summary>
/// <param name="baseCurrencyCode">Base currency code.</param>
/// <param name="targetCurrencyCode">Target currency code.</param>
public class ExchangeRateNotFoundException(string baseCurrencyCode, string targetCurrencyCode)
    : ResourceNotFoundException(
        $"Exchange rate for currency pair '{baseCurrencyCode}'/'{targetCurrencyCode}' not found"
    ) {
}