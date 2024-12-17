namespace CurrencyExchange.Models;

public class Currency
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Sign { get; set; } = null!;
}