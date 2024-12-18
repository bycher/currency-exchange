using CurrencyExchange.Dto;

namespace CurrencyExchange.Services.Interfaces;

public interface IExchangeRateService {
    List<ExchangeRateDto> GetAllExchangeRates();
    ExchangeRateDto? GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode);
    ExchangeRateDto? AddExchangeRate(CreateExchangeRateDto createExchangeRateDto);
    ExchangeRateDto? UpdateExchangeRate(CreateExchangeRateDto createExchangeRateDto);
}