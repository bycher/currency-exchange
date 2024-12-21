using CurrencyExchange.Api.Exceptions;
using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Services;

public sealed class ExchangeService(IExchangeRateService exchangeRateService) : IExchangeService {
    private const string CrossCurrencyCode = "USD";

    /// <summary>
    /// Exchanges amount of money from one currency to another.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Response with exchange result data.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when we can't exchange money neither directly, nor reversed, nor cross.
    /// </exception>
    public ExchangeResultResponse Exchange(
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
    /// <returns>Response with exchange result data or null if exchange rate is not found.</returns>
    private ExchangeResultResponse? DirectExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return TryExecuteExchange(() => {
            var exchangeRate = exchangeRateService.GetExchangeRate(
                baseCurrencyCode, targetCurrencyCode
            );
            return new ExchangeResultResponse {
                BaseCurrency = exchangeRate.BaseCurrency,
                TargetCurrency = exchangeRate.TargetCurrency,
                Rate = exchangeRate.Rate,
                Amount = amount
            };
        });
    }

    /// <summary>
    /// Exchanges amount of money reversed from base currency to target currency.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Response with exchange result data or null if exchange rate is not found.</returns>
    private ExchangeResultResponse? ReversedExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return TryExecuteExchange(() => {
            var reversedExchangeRate = exchangeRateService.GetExchangeRate(
                targetCurrencyCode, baseCurrencyCode
            );
            return new ExchangeResultResponse {
                BaseCurrency = reversedExchangeRate.TargetCurrency,
                TargetCurrency = reversedExchangeRate.BaseCurrency,
                Rate = 1 / reversedExchangeRate.Rate,
                Amount = amount
            };
        });
    }

    /// <summary>
    /// Exchanges amount of money cross-exchanged from base currency to target currency.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <param name="amount">Amount of money to exchange.</param>
    /// <returns>Response with exchange result data or null if exchange rate is not found.</returns>
    private ExchangeResultResponse? CrossExchange(
        string baseCurrencyCode, string targetCurrencyCode, double amount
    ) {
        return TryExecuteExchange(() => {
            var usdToBaseExchangeRate = exchangeRateService.GetExchangeRate(
                CrossCurrencyCode, baseCurrencyCode
            );
            var usdToTargetExchangeRate = exchangeRateService.GetExchangeRate(
                CrossCurrencyCode, targetCurrencyCode
            );
            return new ExchangeResultResponse {
                BaseCurrency = usdToBaseExchangeRate.TargetCurrency,
                TargetCurrency = usdToTargetExchangeRate.TargetCurrency,
                Rate = usdToTargetExchangeRate.Rate / usdToBaseExchangeRate.Rate,
                Amount = amount
            };
        });
    }

    /// <summary>
    /// Tries to execute exchange. Catches exceptions and returns null if exchange rate is not found.
    /// Currency not found exception is thrown further.
    /// </summary>
    /// <param name="exchanger">Exchange function.</param>
    /// <returns>Response with exchange result data or null if exchange rate is not found.</returns>
    private static ExchangeResultResponse? TryExecuteExchange(Func<ExchangeResultResponse> exchanger) {
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