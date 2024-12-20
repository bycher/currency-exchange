namespace CurrencyExchange.Models.Domain;

/// <summary>
/// Currency domain model.
/// </summary>
public class Currency : Entity {
    /// <summary>
    /// Currency code.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Currency full name.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Currency sign.
    /// </summary>
    public required string Sign { get; set; }
}