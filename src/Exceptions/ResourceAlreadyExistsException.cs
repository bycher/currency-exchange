namespace CurrencyExchange.Api.Exceptions;

/// <summary>
/// Exception thrown when a resource already exists (service layer).
/// </summary>
/// <param name="message">Exception message.</param>
/// <param name="innerException">Inner exception.</param>
public class ResourceAlreadyExistsException(string message, Exception innerException)
    : Exception(message, innerException) {
}