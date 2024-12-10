using CurrencyExchange.DataAccess;
using CurrencyExchange.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly CurrenciesRepository _currenciesRepository;

    public CurrenciesController(CurrenciesRepository currenciesRepository)
    {
        _currenciesRepository = currenciesRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Currency>> GetCurrencies()
    {
        return Ok(_currenciesRepository.GetAllCurrencies());
    }
}