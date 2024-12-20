namespace CurrencyExchange.Models.Dto;

/// <summary>
/// Currency DTO.
/// </summary>
/// <param name="Id">Currency ID.</param>
/// <param name="Code">Currency code.</param>
/// <param name="Name">Currency name.</param>
/// <param name="Sign">Currency sign.</param>
public record CurrencyDto(
    int Id,
    string Code,
    string Name,
    string Sign
);