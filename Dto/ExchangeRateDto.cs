namespace CurrencyExchange.Dto;

public class ExchangeRateDto
{
    public required int Id { get; set; }
    public required CurrencyDto BaseCurrency { get; set; }
    public required CurrencyDto TargetCurrency { get; set; }
    public required double Rate { get; set; }
}