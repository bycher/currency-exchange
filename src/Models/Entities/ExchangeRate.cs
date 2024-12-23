namespace CurrencyExchange.Api.Models.Entities;

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
    public decimal Rate { get; set; }
}
