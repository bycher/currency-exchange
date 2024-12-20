using CurrencyExchange.Models.Dto;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Models.Domain;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Repositories.Interfaces;
using AutoMapper;

namespace CurrencyExchange.Services;

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
    public IEnumerable<ExchangeRateDto> GetAllExchangeRates() {
        var exchangeRates = exchangeRatesRepository.GetAllExchangeRates();
        return mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates);
    }

    /// <summary>
    /// Gets an exchange rate for a given currency pair.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <returns>Exchange rate.</returns>
    public ExchangeRateDto GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode) {
        var (baseCurrency, targetCurrency) = GetCurrencyPair(baseCurrencyCode, targetCurrencyCode);

        var exchangeRate = exchangeRatesRepository.GetExchangeRate(baseCurrency.Id, targetCurrency.Id)
            ?? throw new ExchangeRateNotFoundException(baseCurrencyCode, targetCurrencyCode);
            
        return MapExchangeRateDto(exchangeRate, baseCurrency, targetCurrency);
    }

    /// <summary>
    /// Adds a new exchange rate.
    /// </summary>
    /// <param name="exchangeRateForm">Exchange rate data from form.</param>
    /// <returns>Added exchange rate.</returns>
    public ExchangeRateDto AddExchangeRate(ExchangeRateFormDto exchangeRateForm) {
        try {
            var (baseCurrency, targetCurrency) = GetCurrencyPair(
                exchangeRateForm.BaseCurrencyCode, exchangeRateForm.TargetCurrencyCode
            );
            var exchangeRate = MapExchangeRate(exchangeRateForm, baseCurrency, targetCurrency);
            exchangeRate = exchangeRatesRepository.AddExchangeRate(exchangeRate);
            return MapExchangeRateDto(exchangeRate, baseCurrency, targetCurrency);
        }
        catch (DatabaseConflictException ex) {
            throw new ResourceAlreadyExistsException(
                $"Exchange rate for currency pair '{exchangeRateForm.BaseCurrencyCode}'/" +
                $"'{exchangeRateForm.TargetCurrencyCode}' already exists", ex
            );
        }
    }

    /// <summary>
    /// Updates an existing exchange rate.
    /// </summary>
    /// <param name="form">Exchange rate data from form.</param>
    /// <returns>Updated exchange rate.</returns>
    public ExchangeRateDto UpdateExchangeRate(ExchangeRateFormDto form) {
        var (baseCurrency, targetCurrency) = GetCurrencyPair(
            form.BaseCurrencyCode, form.TargetCurrencyCode
        );
        var exchangeRate = MapExchangeRate(form, baseCurrency, targetCurrency);
        exchangeRate = exchangeRatesRepository.UpdateExchangeRate(exchangeRate)
            ?? throw new ExchangeRateNotFoundException(
                form.BaseCurrencyCode, form.TargetCurrencyCode
            );
        return MapExchangeRateDto(exchangeRate, baseCurrency, targetCurrency);
    }

    /// <summary>
    /// Gets a currency pair.
    /// </summary>
    /// <param name="baseCurrencyCode">Base currency code.</param>
    /// <param name="targetCurrencyCode">Target currency code.</param>
    /// <returns>Currency pair.</returns>
    private (CurrencyDto, CurrencyDto) GetCurrencyPair(
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
    private ExchangeRateDto MapExchangeRateDto(
        ExchangeRate exchangeRate, CurrencyDto baseCurrency, CurrencyDto targetCurrency
    ) => mapper.Map<ExchangeRateDto>(exchangeRate) with {
        BaseCurrency = baseCurrency,
        TargetCurrency = targetCurrency
    };

    /// <summary>
    /// Maps an exchange rate form to an exchange rate.
    /// </summary>
    /// <param name="form">Exchange rate form.</param>
    /// <param name="baseCurrency">Base currency (get from currencies service).</param>
    /// <param name="targetCurrency">Target currency (get from currencies service).</param>
    /// <returns>Mapped exchange rate.</returns>
    private ExchangeRate MapExchangeRate(
        ExchangeRateFormDto form, CurrencyDto baseCurrency, CurrencyDto targetCurrency
    ) {
        var exchangeRate = mapper.Map<ExchangeRate>(form);
        exchangeRate.BaseCurrencyId = baseCurrency.Id;
        exchangeRate.TargetCurrencyId = targetCurrency.Id;
        return exchangeRate;
    }
}