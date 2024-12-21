using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;
using CurrencyExchange.Api.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers;

/// <summary>
/// Controller for managing currencies (CRUD operations).
/// </summary>
/// <param name="currencyService">Currency service.</param>
[ApiController]
[Route("api")]
public class CurrenciesController(ICurrencyService currencyService) : ControllerBase {
    /// <summary>
    /// Handles GET requests to retrieve collection of all currencies.
    /// </summary>
    /// <returns>Currencies collection.</returns>
    [HttpGet("currencies")]
    public ActionResult<IEnumerable<CurrencyResponse>> GetCurrencies() {
        var currencyDtos = currencyService.GetAllCurrencies();
        return Ok(currencyDtos);
    }

    /// <summary>
    /// Handles GET requests to retrieve a currency by its code.
    /// </summary>
    /// <param name="code">Currency code.</param>
    /// <returns>Currency.</returns>
    [HttpGet("currency/{code?}")]
    public ActionResult<CurrencyResponse> GetCurrency([ValidCurrencyCode] string? code) {
        if (string.IsNullOrEmpty(code))
            return BadRequest(new ErrorResponse(400, "Currency code is missing."));
        var currency = currencyService.GetCurrency(code);
        return Ok(currency);
    }

    /// <summary>
    /// Handles POST requests to add a new currency.
    /// </summary>
    /// <param name="request">Request with currency creation data.</param>
    /// <returns>Created currency.</returns>
    [HttpPost("currencies")]
    public IActionResult PostCurrency([FromForm] CreateCurrencyRequest request) {
        var currency = currencyService.AddCurrency(request);
        return CreatedAtAction(nameof(GetCurrency), new { code = request.Code }, currency);
    }
}