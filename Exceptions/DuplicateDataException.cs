namespace CurrencyExchange.Exceptions;

public class DuplicateDataException : ServiceException
{
    public DuplicateDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}