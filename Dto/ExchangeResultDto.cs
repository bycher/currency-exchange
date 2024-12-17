namespace CurrencyExchange.Dto;

public class ExchangeResultDto
{
    public required CurrencyDto BaseCurrency { get; set; }
    public required CurrencyDto TargetCurrency { get; set; }
    public required double Rate { get; set; }
    public required double Amount { get; set; }
    public required double ConvertedAmount { get; set; }
}