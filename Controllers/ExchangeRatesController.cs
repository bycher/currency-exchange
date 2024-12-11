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
}