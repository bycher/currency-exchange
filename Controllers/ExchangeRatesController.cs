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

    [HttpPost("exchangeRates")]
    public IActionResult PostExchangeRate([FromForm] ExchangeRateForm? exchangeRateForm)
    {
        if (exchangeRateForm is null)
            return BadRequest("Exchange rate can't be null");
        
        try
        {
            var addedExchangeRate = _exchangeRatesRepository.AddExchangeRate(exchangeRateForm);
            return CreatedAtAction(
                nameof(GetExchangeRate),
                new
                {
                    codePair = exchangeRateForm.BaseCurrencyCode +
                               exchangeRateForm.TargetCurrencyCode
                },
                addedExchangeRate);
        }
        catch (SqliteException ex)
        {
            if (ex.SqliteErrorCode == 19)
                return StatusCode(409, "Exchange rate is already in database");

            return StatusCode(500, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("exchangeRate/{codePair?}")]
    public ActionResult<ExchangeRate> PatchExchangeRate(string? codePair)
    {
        if (string.IsNullOrWhiteSpace(codePair) || codePair.Length != 6)
            return BadRequest("Specify the currency pair in the format 'XXXYYY', " +
                              "where 'XXX', 'YYY' are currency codes");

        var baseCode = codePair[..3];
        var targetCode = codePair[3..];

        if (!Request.Form.TryGetValue("rate", out var newRateRaw))
            return BadRequest("Field 'rate' is missing from the form");
        
        if (!double.TryParse(newRateRaw, out var newRate))
            return BadRequest("Rate must be a floating point number");
        
        try
        {
            var updatedExchangeRate = _exchangeRatesRepository.UpdateExchangeRate(
                baseCode, targetCode, newRate);
            if (updatedExchangeRate == null)
                return NotFound("Currency pair is missing from the database");

            return Ok(updatedExchangeRate);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}