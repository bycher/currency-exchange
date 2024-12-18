using CurrencyExchange.Dto;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Repositories.Interfaces;

namespace CurrencyExchange.Services;

public class ExchangeRateService : IExchangeRateService {
    private readonly IExchangeRatesRepository _exchangeRatesRepository;
    private readonly ICurrencyService _currencyService;

    public ExchangeRateService(
        IExchangeRatesRepository exchangeRatesRepository, ICurrencyService currencyService
    ) {
        _exchangeRatesRepository = exchangeRatesRepository;
        _currencyService = currencyService;
    }

    public List<ExchangeRateDto> GetAllExchangeRates() {
        try {
            var currencyDtosDict = _currencyService.GetAllCurrencies().ToDictionary(c => c.Id);

            return _exchangeRatesRepository.GetAllExchangeRates()
                .Select(er => ToExchangeRateDto(
                    er, currencyDtosDict[er.BaseCurrencyId], currencyDtosDict[er.TargetCurrencyId]
                )!)
                .ToList();
        }
        catch (SqliteException ex) {
            // TODO: add logging
            throw new ServiceException("Failed to get exchange rates", ex);
        }
    }

    public ExchangeRateDto? GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode) {
        try {
            var baseCurrencyDto = _currencyService.GetCurrency(baseCurrencyCode);
            var targetCurrencyDto = _currencyService.GetCurrency(targetCurrencyCode);
            if (baseCurrencyDto == null || targetCurrencyDto == null)
                return null;

            var exchangeRate = _exchangeRatesRepository.GetExchangeRate(
                baseCurrencyDto.Id, targetCurrencyDto.Id
            );
            return ToExchangeRateDto(exchangeRate, baseCurrencyDto, targetCurrencyDto);
        }
        catch (SqliteException ex) {
            // TODO: add logging
            throw new ServiceException("Failed to get exchange rate", ex);
        }
    }

    public ExchangeRateDto? AddExchangeRate(CreateExchangeRateDto createExchangeRateDto) {
        try {
            var baseCurrencyDto = _currencyService.GetCurrency(createExchangeRateDto.BaseCurrencyCode);
            var targetCurrencyDto = _currencyService.GetCurrency(createExchangeRateDto.TargetCurrencyCode);
            if (baseCurrencyDto == null || targetCurrencyDto == null)
                return null;

            var exchangeRate = new ExchangeRate {
                BaseCurrencyId = baseCurrencyDto.Id,
                TargetCurrencyId = targetCurrencyDto.Id,
                Rate = createExchangeRateDto.Rate
            };
            var addedExchangeRate = _exchangeRatesRepository.AddExchangeRate(exchangeRate);
            return ToExchangeRateDto(addedExchangeRate, baseCurrencyDto, targetCurrencyDto);
        }
        catch (SqliteException ex) {
            // TODO: add logging
            if (ex.SqliteErrorCode == 19)
                throw new DuplicateDataException("Exchange rate for currency pair already exists", ex);

            throw new ServiceException("Failed to add the exchange rate", ex);
        }
    }

    public ExchangeRateDto? UpdateExchangeRate(CreateExchangeRateDto createExchangeRateDto) {
        try {
            var baseCurrencyDto = _currencyService.GetCurrency(createExchangeRateDto.BaseCurrencyCode);
            var targetCurrencyDto = _currencyService.GetCurrency(createExchangeRateDto.TargetCurrencyCode);
            if (baseCurrencyDto == null || targetCurrencyDto == null)
                return null;

            var exchangeRate = new ExchangeRate {
                BaseCurrencyId = baseCurrencyDto.Id,
                TargetCurrencyId = targetCurrencyDto.Id,
                Rate = createExchangeRateDto.Rate
            };
            var updatedExchangeRate = _exchangeRatesRepository.UpdateExchangeRate(exchangeRate);
            return ToExchangeRateDto(updatedExchangeRate, baseCurrencyDto, targetCurrencyDto);
        }
        catch (SqliteException ex) {
            // TODO: add logging
            throw new ServiceException("Failed to update the exchange rate", ex);
        }
    }

    private static ExchangeRateDto? ToExchangeRateDto(
        ExchangeRate? exchangeRate, CurrencyDto baseCurrencyDto, CurrencyDto targetCurrencyDto
    ) {
        return exchangeRate != null
            ? new () {
                Id = exchangeRate.Id,
                BaseCurrency = baseCurrencyDto,
                TargetCurrency = targetCurrencyDto,
                Rate = exchangeRate.Rate
            }
            : null;
    }
}