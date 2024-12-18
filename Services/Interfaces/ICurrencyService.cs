using CurrencyExchange.Dto;

namespace CurrencyExchange.Services.Interfaces;

public interface ICurrencyService {
    IEnumerable<CurrencyDto> GetAllCurrencies();
    CurrencyDto? GetCurrency(string currencyCode);
    CurrencyDto AddCurrency(CreateCurrencyDto createCurrencyDto);
}