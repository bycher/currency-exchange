using CurrencyExchange.Models.Dto;

namespace CurrencyExchange.Services.Interfaces;

public interface ICurrencyService {
    IEnumerable<CurrencyDto> GetAllCurrencies();
    CurrencyDto GetCurrency(string code);
    CurrencyDto AddCurrency(CurrencyFormDto currencyForm);
}
