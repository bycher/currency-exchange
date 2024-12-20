using CurrencyExchange.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;

namespace CurrencyExchange.Controllers;

/// <summary>
/// Controller for managing currency exchange operations.
/// </summary>
/// <param name="exchangeService">Exchange service.</param>
[ApiController]
[Route("api/[controller]")]
public class ExchangeController(IExchangeService exchangeService) : ControllerBase {
    /// <summary>
    /// Converts an amount from one currency to another using current exchange rates
    /// </summary>
    /// <param name="from">Source currency code</param>
    /// <param name="to">Target currency code</param>
    /// <param name="amount">Amount of source currency to convert</param>
    /// <returns>A result containing the converted amount and currency pair</returns>
    [HttpGet]
    public ActionResult<ExchangeResultDto> Exchange(
        [ValidCurrencyCode] string from, [ValidCurrencyCode] string to, [GreaterThanZero] double amount
    ) {
        var exchangeResult = exchangeService.Exchange(from, to, amount);
        return Ok(exchangeResult);
    }
}
