using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Interfaces;

public interface IExchangeRateService {
    IEnumerable<ExchangeRateResponse> GetAllExchangeRates();
    ExchangeRateResponse GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode);
    ExchangeRateResponse AddExchangeRate(CreateExchangeRateRequest request);
    ExchangeRateResponse UpdateExchangeRate(CreateExchangeRateRequest request);
}
