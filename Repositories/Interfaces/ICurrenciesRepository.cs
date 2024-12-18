using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories.Interfaces;

public interface ICurrenciesRepository {
    IEnumerable<Currency> GetAllCurrencies();
    Currency? GetCurrency(string currencyCode);
    Currency AddCurrency(Currency currency);
}