using CurrencyExchange.Api.Validation;

namespace CurrencyExchange.Api.Models.Requests;

/// <summary>
/// Currency data form.
/// </summary>
public class CreateCurrencyRequest(string code, string name, string sign) {
    /// <summary>
    /// Currency code.
    /// </summary>
    [ValidCurrencyCode]
    public required string Code { get; init; } = code;

    /// <summary>
    /// Currency name.
    /// </summary>
    public required string Name { get; init; } = name;

    /// <summary>
    /// Currency sign.
    /// </summary>
    public required string Sign { get; init; } = sign;
}