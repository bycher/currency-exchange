using AutoMapper;
using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;
using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Exceptions;
using CurrencyExchange.Api.Models.Entities;

namespace CurrencyExchange.Api.Services;

public sealed class CurrencyService(ICurrenciesRepository currenciesRepository, IMapper mapper)
    : ICurrencyService {
    /// <summary>
    /// Gets all currencies.
    /// </summary>
    /// <returns>All currencies.</returns>
    public IEnumerable<CurrencyResponse> GetAllCurrencies() {
        var currencies = currenciesRepository.GetAllCurrencies();
        return mapper.Map<IEnumerable<CurrencyResponse>>(currencies);
    }

    /// <summary>
    /// Gets a currency by its code.
    /// </summary>
    /// <param name="code">Currency code.</param>
    /// <returns>Response with currency data.</returns>
    /// <exception cref="CurrencyNotFoundException">Thrown when currency is not found.</exception>
    public CurrencyResponse GetCurrency(string code) {
        var currency = currenciesRepository.GetCurrency(code)
            ?? throw new CurrencyNotFoundException(code);
        return mapper.Map<CurrencyResponse>(currency);
    }

    /// <summary>
    /// Adds a new currency.
    /// </summary>
    /// <param name="request">Request with currency creation data.</param>
    /// <returns>Response with added currency data.</returns>
    /// <exception cref="ResourceAlreadyExistsException">Thrown when currency already exists.</exception>
    public CurrencyResponse AddCurrency(CreateCurrencyRequest request) {
        try {
            var currency = mapper.Map<Currency>(request);
            var addedCurrency = currenciesRepository.AddCurrency(currency);
            return mapper.Map<CurrencyResponse>(addedCurrency);
        }
        catch (DatabaseConflictException ex) {
            throw new ResourceAlreadyExistsException(
                $"Currency with code {request.Code} already exists", ex
            );
        }
    }
}