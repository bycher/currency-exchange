using CurrencyExchange.Dto;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Models;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRatesController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    [HttpGet("exchangeRates")]
    public ActionResult<IEnumerable<ExchangeRateDto>> GetAllExchangeRates()
    {
        try
        {
            var exchangeRateDtos = _exchangeRateService.GetAllExchangeRates();
            return Ok(exchangeRateDtos);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("exchangeRate/{codePair?}")]
    public ActionResult<ExchangeRateDto> GetExchangeRate([ValidCurrencyCodePair] string? codePair)
    {
        if (string.IsNullOrEmpty(codePair))
            return BadRequest(new {message = "Currency code pair is missing."});

        var (baseCode, targetCode) = ParseCurrencyCodePair(codePair);
        try
        {
            var exchangeRateDto = _exchangeRateService.GetExchangeRate(baseCode, targetCode);
            if (exchangeRateDto == null)
                return NotFound(new { message = "Exchange rate for the pair was not found" });

            return Ok(exchangeRateDto);
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("exchangeRates")]
    public IActionResult PostExchangeRate([FromForm] CreateExchangeRateDto createExchangeRateDto)
    {
        try
        {
            var addedExchangeRateDto = _exchangeRateService.AddExchangeRate(createExchangeRateDto);
            if (addedExchangeRateDto == null)
                return NotFound(new { message = "Can't find currencies in the database" });

            return CreatedAtAction(
                nameof(GetExchangeRate),
                new
                {
                    codePair = createExchangeRateDto.BaseCurrencyCode +
                               createExchangeRateDto.TargetCurrencyCode
                },
                addedExchangeRateDto);
        }
        catch (DuplicateDataException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPatch("exchangeRate/{codePair?}")]
    public ActionResult<ExchangeRate> PatchExchangeRate(
        [ValidCurrencyCodePair] string? codePair, [FromForm][GreaterThanZero] double rate)
    {
        if (string.IsNullOrEmpty(codePair))
            return BadRequest(new {message = "Currency code pair is missing."});

        var (baseCode, targetCode) = ParseCurrencyCodePair(codePair);
        try
        {
            var updatedExchangeRate = _exchangeRateService.UpdateExchangeRate(baseCode, targetCode, rate);
            if (updatedExchangeRate == null)
                return NotFound(new { message = "Currency pair is missing from the database" });
            return Ok(updatedExchangeRate);
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    private static (string, string) ParseCurrencyCodePair(string codePair)
    {
        return (codePair[..ValidCurrencyCodeAttribute.CodeLength],
                codePair[ValidCurrencyCodeAttribute.CodeLength..]);
    }
}