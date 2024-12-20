namespace CurrencyExchange.Models.Domain;

/// <summary>
/// Exchange rate domain model.
/// </summary>
public class ExchangeRate : Entity {
    /// <summary>
    /// Base currency ID (foreign key).
    /// </summary>
    public int BaseCurrencyId { get; set; }

    /// <summary>
    /// Target currency ID (foreign key).
    /// </summary>
    public int TargetCurrencyId { get; set; }

    /// <summary>
    /// Exchange rate.
    /// </summary>
    public double Rate { get; set; }
}