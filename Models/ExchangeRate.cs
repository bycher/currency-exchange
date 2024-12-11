namespace CurrencyExchange.Models;

public class ExchangeRate
{
    public int ID { get; set; }
    public required Currency BaseCurrency { get; set; }
    public required Currency TargetCurrency { get; set; }
    public double Rate { get; set; }
}