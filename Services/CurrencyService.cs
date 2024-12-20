using AutoMapper;
using CurrencyExchange.Models.Dto;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Repositories.Interfaces;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Models.Domain;

namespace CurrencyExchange.Services;

public sealed class CurrencyService(ICurrenciesRepository currenciesRepository, IMapper mapper)
    : ICurrencyService {
    /// <summary>
    /// Gets all currencies.
    /// </summary>
    /// <returns>All currencies.</returns>
    public IEnumerable<CurrencyDto> GetAllCurrencies() {
        var currencies = currenciesRepository.GetAllCurrencies();
        return mapper.Map<IEnumerable<CurrencyDto>>(currencies);
    }

    /// <summary>
    /// Gets a currency by its code.
    /// </summary>
    /// <param name="code">Currency code.</param>
    /// <returns>Currency.</returns>
    /// <exception cref="CurrencyNotFoundException">Thrown when currency is not found.</exception>
    public CurrencyDto GetCurrency(string code) {
        var currency = currenciesRepository.GetCurrency(code)
            ?? throw new CurrencyNotFoundException(code);
        return mapper.Map<CurrencyDto>(currency);
    }

    /// <summary>
    /// Adds a new currency.
    /// </summary>
    /// <param name="currencyForm">Currency form.</param>
    /// <returns>Added currency.</returns>
    /// <exception cref="ResourceAlreadyExistsException">Thrown when currency already exists.</exception>
    public CurrencyDto AddCurrency(CurrencyFormDto currencyForm) {
        try {
            var currency = mapper.Map<Currency>(currencyForm);
            var addedCurrency = currenciesRepository.AddCurrency(currency);
            return mapper.Map<CurrencyDto>(addedCurrency);
        }
        catch (DatabaseConflictException ex) {
            throw new ResourceAlreadyExistsException(
                $"Currency with code {currencyForm.Code} already exists", ex
            );
        }
    }
}