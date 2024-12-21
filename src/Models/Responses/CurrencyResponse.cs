namespace CurrencyExchange.Api.Models.Responses;

/// <summary>
/// Response with currency data.
/// </summary>
public class CurrencyResponse {
    /// <summary>
    /// Currency ID.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Currency code.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Currency name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Currency sign.
    /// </summary>
    public required string Sign { get; init; }
}
