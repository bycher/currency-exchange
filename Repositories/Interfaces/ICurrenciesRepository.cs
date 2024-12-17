using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories.Interfaces;

public interface ICurrenciesRepository
{
    List<Currency> GetAllCurrencies();
    Currency? GetCurrency(string currencyCode);
    Currency AddCurrency(Currency currency);
}