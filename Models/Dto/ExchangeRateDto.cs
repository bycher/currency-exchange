namespace CurrencyExchange.Models.Dto;

/// <summary>
/// Exchange rate DTO.
/// </summary>
/// <param name="Id">Exchange rate ID.</param>
/// <param name="BaseCurrency">Base currency.</param>
/// <param name="TargetCurrency">Target currency.</param>
/// <param name="Rate">Exchange rate.</param>
public record ExchangeRateDto(
    int Id,
    CurrencyDto BaseCurrency,
    CurrencyDto TargetCurrency,
    double Rate
);