using CurrencyExchange.Dto;
using CurrencyExchange.Services.Interfaces;

namespace CurrencyExchange.Services;

public class ExchangeService : IExchangeService {
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeService(IExchangeRateService exchangeRateService) {
        _exchangeRateService = exchangeRateService;
    }

    public ExchangeResultDto? Exchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        var directExchangeResult = DirectExchange(baseCurrencyCode, targetCurrencyCode, amount);
        if (directExchangeResult != null)
            return directExchangeResult;

        var reverseExchangeResult = ReversedExchange(baseCurrencyCode, targetCurrencyCode, amount);
        if (reverseExchangeResult != null)
            return reverseExchangeResult;

        var crossExchangeResult = CrossExchange(baseCurrencyCode, targetCurrencyCode, amount);
        if (crossExchangeResult != null)
            return crossExchangeResult;

        return null;
    }

    private ExchangeResultDto? DirectExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        var exchangeRate = _exchangeRateService.GetExchangeRate(baseCurrencyCode, targetCurrencyCode);
        
        return exchangeRate == null
            ? null
            : new ExchangeResultDto {
                BaseCurrency = exchangeRate.BaseCurrency,
                TargetCurrency = exchangeRate.TargetCurrency,
                Rate = exchangeRate.Rate,
                Amount = amount,
                ConvertedAmount = exchangeRate.Rate * amount
            };
    }

    private ExchangeResultDto? ReversedExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        var reversedExchangeRate = _exchangeRateService.GetExchangeRate(targetCurrencyCode, baseCurrencyCode);

        return reversedExchangeRate == null
            ? null
            : new ExchangeResultDto {
                BaseCurrency = reversedExchangeRate.TargetCurrency,
                TargetCurrency = reversedExchangeRate.BaseCurrency,
                Rate = 1 / reversedExchangeRate.Rate,
                Amount = amount,
                ConvertedAmount = amount / reversedExchangeRate.Rate
            };
    }

    private ExchangeResultDto? CrossExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        var usdToBaseExchangeRate = _exchangeRateService.GetExchangeRate("USD", baseCurrencyCode);
        var usdToTargetExchangeRate = _exchangeRateService.GetExchangeRate("USD", targetCurrencyCode);

        return usdToTargetExchangeRate == null || usdToBaseExchangeRate == null
            ? null
            : new ExchangeResultDto {
                BaseCurrency = usdToBaseExchangeRate.TargetCurrency,
                TargetCurrency = usdToTargetExchangeRate.TargetCurrency,
                Rate = usdToTargetExchangeRate.Rate / usdToBaseExchangeRate.Rate,
                Amount = amount,
                ConvertedAmount = amount * usdToTargetExchangeRate.Rate / usdToBaseExchangeRate.Rate
            };
    }
}