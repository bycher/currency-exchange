namespace CurrencyExchange.Models.Domain;

/// <summary>
/// Base class for all domain models.
/// </summary>
public class Entity {
    /// <summary>
    /// Entity ID (primary key).
    /// </summary>
    public int Id { get; set; }
}