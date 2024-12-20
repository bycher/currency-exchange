namespace CurrencyExchange.Exceptions;

/// <summary>
/// Exception thrown when a database conflict occurs (repository layer).
/// </summary>
/// <param name="message">Exception message.</param>
/// <param name="innerException">Inner exception.</param>
public class DatabaseConflictException(string message, Exception innerException)
    : Exception(message, innerException) {
}