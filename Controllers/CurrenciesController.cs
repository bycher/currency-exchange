using CurrencyExchange.Models.Dto;
using CurrencyExchange.Models.Responses;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers;

/// <summary>
/// Controller for managing currencies (CRUD operations).
/// </summary>
/// <param name="currencyService">Currency service.</param>
[ApiController]
[Route("api")]
public class CurrenciesController(ICurrencyService currencyService) : ControllerBase {
    /// <summary>
    /// Handles GET requests to retrieve all currencies.
    /// </summary>
    /// <returns>Currencies collection.</returns>
    [HttpGet("currencies")]
    public ActionResult<IEnumerable<CurrencyDto>> GetCurrencies() {
        var currencyDtos = currencyService.GetAllCurrencies();
        return Ok(currencyDtos);
    }

    /// <summary>
    /// Handles GET requests to retrieve a currency by its code.
    /// </summary>
    /// <param name="code">Currency code.</param>
    /// <returns>Currency.</returns>
    [HttpGet("currency/{code?}")]
    public ActionResult<CurrencyDto> GetCurrency([ValidCurrencyCode] string? code) {
        if (string.IsNullOrEmpty(code))
            return BadRequest(new ErrorResponse(400, "Currency code is missing."));

        var currency = currencyService.GetCurrency(code);
        return Ok(currency);
    }

    /// <summary>
    /// Handles POST requests to add a new currency.
    /// </summary>
    /// <param name="createCurrencyDto">Currency form data.</param>
    /// <returns>Created currency.</returns>
    [HttpPost("currencies")]
    public IActionResult PostCurrency([FromForm] CurrencyFormDto createCurrencyDto) {
        var addedCurrency = currencyService.AddCurrency(createCurrencyDto);
        return CreatedAtAction(
            nameof(GetCurrency), new { code = createCurrencyDto.Code }, addedCurrency
        );
    }
}