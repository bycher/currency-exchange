namespace CurrencyExchange.Models;

public class Currency
{
    public int ID { get; set; }
    public required string Code { get; set; }
    public required string FullName { get; set; }
    public required string Sign { get; set; }
}