using CurrencyExchange.Validation;

namespace CurrencyExchange.Dto;

public class CreateExchangeRateDto {
    [ValidCurrencyCode]
    public required string BaseCurrencyCode { get; set; }
    [ValidCurrencyCode]
    public required string TargetCurrencyCode { get; set; }
    [GreaterThanZero]
    public required double Rate { get; set; }
}