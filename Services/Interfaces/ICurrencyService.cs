using CurrencyExchange.Dto;

namespace CurrencyExchange.Services.Interfaces;

public interface ICurrencyService {
    List<CurrencyDto> GetAllCurrencies();
    CurrencyDto? GetCurrency(string currencyCode);
    CurrencyDto AddCurrency(CreateCurrencyDto createCurrencyDto);
}