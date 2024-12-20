namespace CurrencyExchange.Exceptions;

/// <summary>
/// Exception thrown when a database error occurs (repository layer).
/// </summary>
/// <param name="message">Exception message.</param>
/// <param name="innerException">Inner exception.</param>
public class DatabaseException(string message, Exception innerException)
    : Exception(message, innerException) {
}