using CurrencyExchange.Models.Dto;

namespace CurrencyExchange.Services.Interfaces;

public interface IExchangeRateService {
    IEnumerable<ExchangeRateDto> GetAllExchangeRates();
    ExchangeRateDto GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode);
    ExchangeRateDto AddExchangeRate(ExchangeRateFormDto createExchangeRateDto);
    ExchangeRateDto UpdateExchangeRate(ExchangeRateFormDto createExchangeRateDto);
}