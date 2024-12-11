using CurrencyExchange.DataAccess;
using CurrencyExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api")]
public class ExchangeRatesController : ControllerBase
{
    private readonly ExchangeRatesRepository _exchangeRatesRepository;

    public ExchangeRatesController(ExchangeRatesRepository exchangeRatesRepository)
    {
        _exchangeRatesRepository = exchangeRatesRepository;
    }

    [HttpGet("exchangeRates")]
    public ActionResult<IEnumerable<ExchangeRate>> GetAllExchangeRates()
    {
        try
        {
            var exchangeRates = _exchangeRatesRepository.GetAllExchangeRates();
            return Ok(exchangeRates);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("exchangeRate/{codePair?}")]
    public ActionResult<ExchangeRate> GetExchangeRate(string? codePair)
    {
        if (string.IsNullOrWhiteSpace(codePair) || codePair.Length != 6)
            return BadRequest("Specify the currency pair in the format 'XXXYYY', " +
                              "where 'XXX', 'YYY' are currency codes");

        var baseCode = codePair[..3];
        var targetCode = codePair[3..];

        ExchangeRate? exchangeRate;
        try
        {
            exchangeRate = _exchangeRatesRepository.GetExchangeRate(baseCode, targetCode);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, ex.Message);
        }
        if (exchangeRate is null)
            return NotFound("Exchange rate for the pair was not found");
        
        return Ok(exchangeRate);
    }
}