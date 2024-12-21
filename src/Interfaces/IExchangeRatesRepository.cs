using CurrencyExchange.Api.Models.Entities;

namespace CurrencyExchange.Api.Interfaces;

public interface IExchangeRatesRepository {
    IEnumerable<ExchangeRate> GetAllExchangeRates();
    ExchangeRate? GetExchangeRate(int baseCurrencyId, int targetCurrencyId);
    ExchangeRate AddExchangeRate(ExchangeRate exchangeRate);
    ExchangeRate? UpdateExchangeRate(ExchangeRate exchangeRate);
}