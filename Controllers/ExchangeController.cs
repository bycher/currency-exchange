using CurrencyExchange.DataAccess;
using CurrencyExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly ExchangeRatesRepository _exchangeRatesRepository;

    public ExchangeController(ExchangeRatesRepository exchangeRatesRepository)
    {
        _exchangeRatesRepository = exchangeRatesRepository;
    }

    [HttpGet]
    public ActionResult<ExchangeResult> GetExchange(string from, string to, double amount)
    {
        ExchangeResult? exchangeResult;
        try
        {
            exchangeResult = _exchangeRatesRepository.Exchange(from, to, amount);
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, ex.Message);
        }

        if (exchangeResult == null)
            return NotFound("Сan't calculate the exchange rate for a currency pair");

        return Ok(exchangeResult);
    }
}