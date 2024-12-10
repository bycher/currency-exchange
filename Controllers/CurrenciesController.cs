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
            return BadRequest("The currency code is missing from the address");

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

    
}