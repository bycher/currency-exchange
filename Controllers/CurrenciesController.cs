using CurrencyExchange.DataAccess;
using CurrencyExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api")]
public class CurrenciesController : ControllerBase
{
    private readonly CurrenciesRepository _currenciesRepository;

    public CurrenciesController(CurrenciesRepository currenciesRepository)
    {
        _currenciesRepository = currenciesRepository;
    }

    [HttpGet("currencies")]
    public ActionResult<IEnumerable<Currency>> GetCurrencies()
    {
        return Ok(_currenciesRepository.GetAllCurrencies());
    }

    [HttpGet("currency/{code}")]
    public ActionResult<Currency> GetCurrency(string? code)
    {
        if (code is null)
            return BadRequest("Currency code is missing");

        Currency? currency;
        try
        {
            currency = _currenciesRepository.GetCurrency(code);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, ex.Message);
        } 
        if (currency == null)
            return NotFound();
        
        return currency;
    }

    [HttpPost("currencies")]
    public IActionResult PostCurrency([FromForm] Currency? currency)
    {
        if (currency is null)
            return BadRequest("Currency is null");
            
        Currency? addedCurrency = null;
        try
        {
            addedCurrency = _currenciesRepository.AddCurrency(currency);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, ex.Message);
        }

        if (addedCurrency is null)
            return StatusCode(409, "Currency is already in database");
        
        return CreatedAtAction(nameof(GetCurrency), new { code = currency.Code }, addedCurrency);
    }
}