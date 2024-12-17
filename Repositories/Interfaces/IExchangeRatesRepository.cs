using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories.Interfaces;

public interface IExchangeRatesRepository
{
    List<ExchangeRate> GetAllExchangeRates();
    ExchangeRate? GetExchangeRate(int baseCurrencyId, int targetCurrencyId);
    ExchangeRate AddExchangeRate(ExchangeRate exchangeRate);
    ExchangeRate? UpdateExchangeRate(int baseCurrencyId, int targetCurrencyId, double newRate);
}