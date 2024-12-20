namespace CurrencyExchange.Models.Dto;

/// <summary>
/// Exchange result DTO.
/// </summary>
/// <param name="BaseCurrency">Base currency.</param>
/// <param name="TargetCurrency">Target currency.</param>
/// <param name="Rate">Exchange rate.</param>
/// <param name="Amount">Amount to exchange.</param>
/// <param name="ConvertedAmount">Converted amount.</param>
public record ExchangeResultDto(
    CurrencyDto BaseCurrency,
    CurrencyDto TargetCurrency,
    double Rate,
    double Amount
) {
    public double ConvertedAmount => Rate * Amount;
}
