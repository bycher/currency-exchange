namespace CurrencyExchange.Models;

public record ExchangeRate : Entity {
    public int BaseCurrencyId { get; init; }
    public int TargetCurrencyId { get; init; }
    public double Rate { get; init; }
}