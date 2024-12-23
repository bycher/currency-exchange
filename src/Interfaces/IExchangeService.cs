using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Interfaces;

public interface IExchangeService {
    ExchangeResultResponse Exchange(string baseCurrencyCode, string targetCurrencyCode, decimal amount);
}
