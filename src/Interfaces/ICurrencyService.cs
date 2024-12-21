using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Interfaces;

public interface ICurrencyService {
    IEnumerable<CurrencyResponse> GetAllCurrencies();
    CurrencyResponse GetCurrency(string code);
    CurrencyResponse AddCurrency(CreateCurrencyRequest currencyForm);
}
