using CurrencyExchange.Dto;
using CurrencyExchange.Exceptions;
using Microsoft.AspNetCore.Mvc;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController : ControllerBase {
    private readonly IExchangeService _exchangeService;

    public ExchangeController(IExchangeService exchangeService) {
        _exchangeService = exchangeService;
    }

    [HttpGet]
    public ActionResult<ExchangeResultDto> Exchange(
        [ValidCurrencyCode] string from, [ValidCurrencyCode] string to, [GreaterThanZero] double amount
    ) {
        try {
            var exchangeResult = _exchangeService.Exchange(from, to, amount);
            if (exchangeResult == null)
                return NotFound(new { message = "Сan't calculate exchange rate for a currency pair" });

            return Ok(exchangeResult);
        }
        catch (ServiceException ex) {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}