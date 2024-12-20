namespace CurrencyExchange.Exceptions;

/// <summary>
/// Exception thrown when some resource is not found (service layer).
/// </summary>
/// <param name="message">Exception message.</param>
public class ResourceNotFoundException(string message) : Exception(message) {
}