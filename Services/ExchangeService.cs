using CurrencyExchange.Exceptions;
using CurrencyExchange.Models.Dto;
using CurrencyExchange.Services.Interfaces;

namespace CurrencyExchange.Services;

public sealed class ExchangeService(IExchangeRateService exchangeRateService) : IExchangeService {
    private const string CrossCurrencyCode = "USD";

    /// <summary>
    /// Exchanges amount of money from one currency to another.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Exchange result.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when we can't exchange money neither directly, nor reversed, nor cross.
    /// </exception>
    public ExchangeResultDto Exchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return DirectExchange(baseCurrencyCode, targetCurrencyCode, amount)
            ?? ReversedExchange(baseCurrencyCode, targetCurrencyCode, amount)
            ?? CrossExchange(baseCurrencyCode, targetCurrencyCode, amount)
            ?? throw new ResourceNotFoundException(
                $"Can't exchange '{baseCurrencyCode}' -> '{targetCurrencyCode}'" +
                " according to available exchange rates"
            );
    }

    /// <summary>
    /// Exchanges amount of money directly from base currency to target currency.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Exchange result or null if exchange rate is not found.</returns>
    private ExchangeResultDto? DirectExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return TryExecuteExchange(() => {
            var exchangeRate = exchangeRateService.GetExchangeRate(
                baseCurrencyCode, targetCurrencyCode
            );
            return new ExchangeResultDto(
                exchangeRate.BaseCurrency,
                exchangeRate.TargetCurrency,
                exchangeRate.Rate,
                amount
            );
        });
    }

    /// <summary>
    /// Exchanges amount of money reversed from base currency to target currency.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Exchange result or null if exchange rate is not found.</returns>
    private ExchangeResultDto? ReversedExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return TryExecuteExchange(() => {
            var reversedExchangeRate = exchangeRateService.GetExchangeRate(
                targetCurrencyCode, baseCurrencyCode
            );
            return new ExchangeResultDto(
                reversedExchangeRate.TargetCurrency,
                reversedExchangeRate.BaseCurrency,
                1 / reversedExchangeRate.Rate,
                amount
            );
        });
    }

    /// <summary>
    /// Exchanges amount of money cross-exchanged from base currency to target currency.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Exchange result or null if exchange rate is not found.</returns>
    private ExchangeResultDto? CrossExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return TryExecuteExchange(() => {
            var usdToBaseExchangeRate = exchangeRateService.GetExchangeRate(
                CrossCurrencyCode, baseCurrencyCode
            );
            var usdToTargetExchangeRate = exchangeRateService.GetExchangeRate(
                CrossCurrencyCode, targetCurrencyCode
            );
            return new ExchangeResultDto(
                usdToBaseExchangeRate.TargetCurrency,
                usdToTargetExchangeRate.TargetCurrency,
                usdToTargetExchangeRate.Rate / usdToBaseExchangeRate.Rate,
                amount
            );
        });
    }

    /// <summary>
    /// Tries to execute exchange. Catches exceptions and returns null if exchange rate is not found.
    /// Currency not found exception is thrown further.
    /// </summary>
    /// <param name="exchanger">Exchange function.</param>
    /// <returns>Exchange result or null if exchange rate is not found.</returns>
    private static ExchangeResultDto? TryExecuteExchange(Func<ExchangeResultDto> exchanger) {
        try {
            return exchanger();
        }
        catch (CurrencyNotFoundException) {
            throw;
        }
        catch (ExchangeRateNotFoundException) {
            return null;
        }
    }
}