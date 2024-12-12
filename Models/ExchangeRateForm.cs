namespace CurrencyExchange.Models;

public class ExchangeRateForm
{
    public required string BaseCurrencyCode { get; set; }
    public required string TargetCurrencyCode { get; set; }
    public double Rate { get; set; }
}