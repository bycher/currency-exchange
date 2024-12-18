using AutoMapper;
using CurrencyExchange.Dto;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;
using CurrencyExchange.Repositories.Interfaces;
using CurrencyExchange.Services.Interfaces;

namespace CurrencyExchange.Services;

public class CurrencyService : ICurrencyService {
    private readonly ICurrenciesRepository _currenciesRepository;
    private readonly IMapper _mapper;

    public CurrencyService(ICurrenciesRepository currenciesRepository, IMapper mapper) {
        _currenciesRepository = currenciesRepository;
        _mapper = mapper;
    }

    public IEnumerable<CurrencyDto> GetAllCurrencies() {
        try {
            return _currenciesRepository.GetAllCurrencies()
                .Select(_mapper.Map<CurrencyDto>);
        }
        catch (SqliteException ex) {
            // TODO: add logging
            throw new ServiceException("Failed to get currencies", ex);
        }
    }

    public CurrencyDto? GetCurrency(string code) {
        try {
            var currency = _currenciesRepository.GetCurrency(code);
            return currency != null
                ? _mapper.Map<CurrencyDto>(currency)
                : null;
        }
        catch (SqliteException ex) {
            // TODO: add logging
            throw new ServiceException("Failed to get currency", ex);
        }
    }

    public CurrencyDto AddCurrency(CreateCurrencyDto createCurrencyDto) {
        try {
            var currency = _mapper.Map<Currency>(createCurrencyDto);
            var addedCurrency = _currenciesRepository.AddCurrency(currency);
            return _mapper.Map<CurrencyDto>(addedCurrency);
        }
        catch (SqliteException ex) {
            // TODO: add logging
            if (ex.SqliteErrorCode == 19)
                throw new DuplicateDataException("Currency already exists", ex);

            throw new ServiceException("Failed to get currency", ex);
        }
    }
}