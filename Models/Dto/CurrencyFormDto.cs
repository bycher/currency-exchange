using CurrencyExchange.Validation;

namespace CurrencyExchange.Models.Dto;

/// <summary>
/// Currency data form.
/// </summary>
/// <param name="Code">Currency code.</param>
/// <param name="Name">Currency name.</param>
/// <param name="Sign">Currency sign.</param>
public record CurrencyFormDto(
    [ValidCurrencyCode] string Code,
    string Name,
    string Sign
);