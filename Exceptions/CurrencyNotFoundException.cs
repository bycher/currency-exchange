namespace CurrencyExchange.Exceptions;

/// <summary>
/// Exception thrown when a currency is not found (service layer).
/// Provides user-friendly message.
/// </summary>
/// <param name="currencyCode">Currency code.</param>
public class CurrencyNotFoundException(string currencyCode)
    : ResourceNotFoundException($"Currency with code '{currencyCode}' not found") {
}