using CurrencyExchange.Api.Models.Entities;

namespace CurrencyExchange.Api.Interfaces;

public interface ICurrenciesRepository {
    IEnumerable<Currency> GetAllCurrencies();
    Currency? GetCurrency(string currencyCode);
    Currency AddCurrency(Currency currency);
}