namespace CurrencyExchange.Models;

public class ExchangeResult
{
    public required Currency BaseCurrency { get; set; }
    public required Currency TargetCurrency { get; set; }
    public double Rate { get; set; }
    public double Amount { get; set; }
    public double ConvertedAmount { get; set; }
}