using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories.Interfaces;

public interface IExchangeRatesRepository {
    IEnumerable<ExchangeRate> GetAllExchangeRates();
    ExchangeRate? GetExchangeRate(int baseCurrencyId, int targetCurrencyId);
    ExchangeRate AddExchangeRate(ExchangeRate exchangeRate);
    ExchangeRate? UpdateExchangeRate(ExchangeRate exchangeRate);
}