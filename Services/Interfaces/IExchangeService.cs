using CurrencyExchange.Models.Dto;

namespace CurrencyExchange.Services.Interfaces;

public interface IExchangeService {
    ExchangeResultDto Exchange(string baseCurrencyCode, string targetCurrencyCode, double amount);
}