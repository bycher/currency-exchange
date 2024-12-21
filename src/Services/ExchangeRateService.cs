using AutoMapper;
using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;
using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Exceptions;
using CurrencyExchange.Api.Models.Entities;

namespace CurrencyExchange.Api.Services;

/// <summary>
/// Service for managing exchange rates.
/// </summary>
/// <param name="exchangeRatesRepository">Exchange rates repository.</param>
/// <param name="currenciesService">Currencies service.</param>
/// <param name="mapper">Mapper.</param>
public sealed class ExchangeRateService(
    IExchangeRatesRepository exchangeRatesRepository,
    ICurrencyService currenciesService,
    IMapper mapper
) : IExchangeRateService {
    /// <summary>
    /// Gets all exchange rates.
    /// </summary>
    /// <returns>All exchange rates.</returns>
    public IEnumerable<ExchangeRateResponse> GetAllExchangeRates() {
        var exchangeRates = exchangeRatesRepository.GetAllExchangeRates();
        return mapper.Map<IEnumerable<ExchangeRateResponse>>(exchangeRates);
    }

    /// <summary>
    /// Gets an exchange rate for a given currency pair.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <returns>Exchange rate.</returns>
    public ExchangeRateResponse GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode) {
        var (baseCurrency, targetCurrency) = GetCurrencyPair(baseCurrencyCode, targetCurrencyCode);

        var exchangeRate = exchangeRatesRepository.GetExchangeRate(baseCurrency.Id, targetCurrency.Id)
            ?? throw new ExchangeRateNotFoundException(baseCurrencyCode, targetCurrencyCode);
            
        return MapExchangeRateDto(exchangeRate, baseCurrency, targetCurrency);
    }

    /// <summary>
    /// Adds a new exchange rate.
    /// </summary>
    /// <param name="request">Request with exchange rate creation data.</param>
    /// <returns>Response with added exchange rate data.</returns>
    public ExchangeRateResponse AddExchangeRate(CreateExchangeRateRequest request) {
        try {
            var (baseCurrency, targetCurrency) = GetCurrencyPair(
                request.BaseCurrencyCode, request.TargetCurrencyCode
            );
            var exchangeRate = MapExchangeRate(request, baseCurrency, targetCurrency);
            exchangeRate = exchangeRatesRepository.AddExchangeRate(exchangeRate);
            return MapExchangeRateDto(exchangeRate, baseCurrency, targetCurrency);
        }
        catch (DatabaseConflictException ex) {
            throw new ResourceAlreadyExistsException(
                $"Exchange rate for currency pair '{request.BaseCurrencyCode}'/" +
                $"'{request.TargetCurrencyCode}' already exists", ex
            );
        }
    }

    /// <summary>
    /// Updates an existing exchange rate.
    /// </summary>
    /// <param name="request">Request with exchange rate creation data.</param>
    /// <returns>Response with updated exchange rate data.</returns>
    public ExchangeRateResponse UpdateExchangeRate(CreateExchangeRateRequest request) {
        var (baseCurrency, targetCurrency) = GetCurrencyPair(
            request.BaseCurrencyCode, request.TargetCurrencyCode
        );
        var exchangeRate = MapExchangeRate(request, baseCurrency, targetCurrency);
        exchangeRate = exchangeRatesRepository.UpdateExchangeRate(exchangeRate)
            ?? throw new ExchangeRateNotFoundException(
                request.BaseCurrencyCode, request.TargetCurrencyCode
            );
        return MapExchangeRateDto(exchangeRate, baseCurrency, targetCurrency);
    }

    /// <summary>
    /// Gets a currency pair.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <returns>Currency pair.</returns>
    private (CurrencyResponse, CurrencyResponse) GetCurrencyPair(
        string baseCurrencyCode, string targetCurrencyCode
    ) => (
        currenciesService.GetCurrency(baseCurrencyCode),
        currenciesService.GetCurrency(targetCurrencyCode)
    );

    /// <summary>
    /// Maps an exchange rate to a DTO.
    /// </summary>
    /// <param name="exchangeRate">Exchange rate.</param>
    /// <param name="baseCurrency">Base currency (get from currencies service).</param>
    /// <param name="targetCurrency">Target currency (get from currencies service).</param>
    /// <returns>Mapped exchange rate.</returns>
    private ExchangeRateResponse MapExchangeRateDto(
        ExchangeRate exchangeRate, CurrencyResponse baseCurrency, CurrencyResponse targetCurrency
    ) {
        var exchangeRateResponse = mapper.Map<ExchangeRateResponse>(exchangeRate);
        exchangeRateResponse.BaseCurrency = baseCurrency;
        exchangeRateResponse.TargetCurrency = targetCurrency;
        return exchangeRateResponse;
    }

    /// <summary>
    /// Maps an exchange rate creation request to an exchange rate.
    /// </summary>
    /// <param name="request">Request with exchange rate creation data.</param>
    /// <param name="baseCurrency">Base currency (get from currencies service).</param>
    /// <param name="targetCurrency">Target currency (get from currencies service).</param>
    /// <returns>Mapped exchange rate.</returns>
    private ExchangeRate MapExchangeRate(
        CreateExchangeRateRequest request, CurrencyResponse baseCurrency, CurrencyResponse targetCurrency
    ) {
        var exchangeRate = mapper.Map<ExchangeRate>(request);
        exchangeRate.BaseCurrencyId = baseCurrency.Id;
        exchangeRate.TargetCurrencyId = targetCurrency.Id;
        return exchangeRate;
    }
}