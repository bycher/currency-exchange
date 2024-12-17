using CurrencyExchange.Dto;
using CurrencyExchange.Services.Interfaces;

namespace CurrencyExchange.Services;

public class ExchangeService : IExchangeService
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeService(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public ExchangeResultDto? Exchange(string baseCurrencyCode, string targetCurrencyCode, double amount)
    {
        var directExchangeResult = DirectExchange(baseCurrencyCode, targetCurrencyCode, amount);
        if (directExchangeResult != null)
            return directExchangeResult;

        var reverseExchangeResult = ReverseExchange(baseCurrencyCode, targetCurrencyCode, amount);
        if (reverseExchangeResult != null)
            return reverseExchangeResult;

        var crossExchangeResult = CrossExchange(baseCurrencyCode, targetCurrencyCode, amount);
        if (crossExchangeResult != null)
            return crossExchangeResult;

        return null;
    }

    private ExchangeResultDto? DirectExchange(string baseCurrencyCode, string targetCurrencyCode, double amount)
    {
        var exchangeRateDto = _exchangeRateService.GetExchangeRate(baseCurrencyCode, targetCurrencyCode);
        if (exchangeRateDto == null)
            return null;

        return new ExchangeResultDto
        {
            BaseCurrency = exchangeRateDto.BaseCurrency,
            TargetCurrency = exchangeRateDto.TargetCurrency,
            Rate = exchangeRateDto.Rate,
            Amount = amount,
            ConvertedAmount = exchangeRateDto.Rate * amount
        };
    }

    private ExchangeResultDto? ReverseExchange(string baseCurrencyCode, string targetCurrencyCode, double amount)
    {
        var exchangeRateDto = _exchangeRateService.GetExchangeRate(targetCurrencyCode, baseCurrencyCode);
        if (exchangeRateDto == null)
            return null;

        return new ExchangeResultDto
        {
            BaseCurrency = exchangeRateDto.TargetCurrency,
            TargetCurrency = exchangeRateDto.BaseCurrency,
            Rate = 1 / exchangeRateDto.Rate,
            Amount = amount,
            ConvertedAmount = amount / exchangeRateDto.Rate
        };
    }

    private ExchangeResultDto? CrossExchange(string baseCurrencyCode, string targetCurrencyCode, double amount)
    {
        var usdToBaseExchangeRateDto = _exchangeRateService.GetExchangeRate("USD", baseCurrencyCode);
        if (usdToBaseExchangeRateDto == null)
            return null;
        var usdToTargetExchangeRateDto = _exchangeRateService.GetExchangeRate("USD", targetCurrencyCode);
        if (usdToTargetExchangeRateDto == null)
            return null;

        return new ExchangeResultDto
        {
            BaseCurrency = usdToBaseExchangeRateDto.TargetCurrency,
            TargetCurrency = usdToTargetExchangeRateDto.TargetCurrency,
            Rate = usdToTargetExchangeRateDto.Rate / usdToBaseExchangeRateDto.Rate,
            Amount = amount,
            ConvertedAmount = amount * usdToTargetExchangeRateDto.Rate / usdToBaseExchangeRateDto.Rate
        };
    }
}