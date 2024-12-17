using CurrencyExchange.Validation;

namespace CurrencyExchange.Dto;

public class CreateCurrencyDto
{
    [ValidCurrencyCode]
    public required string Code { get; set; }
    public required string FullName { get; set; }
    public required string Sign { get; set; }
}