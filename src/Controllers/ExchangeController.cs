using Microsoft.AspNetCore.Mvc;
using CurrencyExchange.Api.Validation;
using CurrencyExchange.Api.Models.Responses;
using CurrencyExchange.Api.Interfaces;

namespace CurrencyExchange.Api.Controllers;

/// <summary>
/// Controller for managing currency exchange operations.
/// </summary>
/// <param name="exchangeService">Exchange service.</param>
[ApiController]
[Route("api/[controller]")]
public class ExchangeController(IExchangeService exchangeService) : ControllerBase {
    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    /// <param name="from">Base currency code</param>
    /// <param name="to">Target currency code</param>
    /// <param name="amount">Amount of base currency to convert</param>
    /// <returns>A result containing the converted amount and currency pair</returns>
    [HttpGet]
    public ActionResult<ExchangeResultResponse> Exchange(
        [ValidCurrencyCode] string from, [ValidCurrencyCode] string to, [GreaterThanZero] decimal amount
    ) {
        var exchangeResult = exchangeService.Exchange(from, to, amount);
        return Ok(exchangeResult);
    }
}
