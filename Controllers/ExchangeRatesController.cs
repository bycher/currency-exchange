using CurrencyExchange.Models.Dto;
using CurrencyExchange.Models.Responses;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers;

/// <summary>
/// Controller for managing exchange rates (CRUD operations).
/// </summary>
/// <param name="exchangeRateService">Exchange rate service.</param>
[ApiController]
[Route("api")]
public class ExchangeRatesController(IExchangeRateService exchangeRateService) : ControllerBase {
    /// <summary>
    /// Handles GET requests to retrieve all exchange rates.
    /// </summary>
    /// <returns>Exchange rates collection.</returns>
    [HttpGet("exchangeRates")]
    public ActionResult<IEnumerable<ExchangeRateDto>> GetAllExchangeRates() {
        var exchangeRates = exchangeRateService.GetAllExchangeRates();
        return Ok(exchangeRates);
    }

    /// <summary>
    /// Handles GET requests to retrieve an exchange rate for a given currency pair.
    /// </summary>
    /// <param name="codePair">Currency pair code (e.g. "USDEUR").</param>
    /// <returns>Exchange rate.</returns>
    [HttpGet("exchangeRate/{codePair?}")]
    public ActionResult<ExchangeRateDto> GetExchangeRate([ValidCurrencyCodePair] string? codePair) {
        if (string.IsNullOrEmpty(codePair))
            return BadRequest(new ErrorResponse(400, "Currency code pair is missing."));

        var (baseCode, targetCode) = ParseCurrencyCodePair(codePair);
        var exchangeRate = exchangeRateService.GetExchangeRate(baseCode, targetCode);
        return Ok(exchangeRate);
    }

    /// <summary>
    /// Handles POST requests to add an exchange rate.
    /// </summary>
    /// <param name="exchangeRateForm">Exchange rate form.</param>
    /// <returns>Created exchange rate.</returns>
    [HttpPost("exchangeRates")]
    public IActionResult PostExchangeRate([FromForm] ExchangeRateFormDto exchangeRateForm) {
        var exchangeRate = exchangeRateService.AddExchangeRate(exchangeRateForm);
        return CreatedAtAction(
            nameof(GetExchangeRate),
            new {
                codePair = exchangeRateForm.BaseCurrencyCode +
                           exchangeRateForm.TargetCurrencyCode
            },
            exchangeRate
        );
    }

    /// <summary>
    /// Handles PATCH requests to update an exchange rate.
    /// </summary>
    /// <param name="codePair">Currency pair code (e.g. "USDEUR").</param>
    /// <param name="rate">Exchange rate.</param>
    /// <returns>Updated exchange rate.</returns>
    [HttpPatch("exchangeRate/{codePair?}")]
    public ActionResult<ExchangeRateDto> PatchExchangeRate(
        [ValidCurrencyCodePair] string? codePair, [FromForm][GreaterThanZero] double rate
    ) {
        if (string.IsNullOrEmpty(codePair))
            return BadRequest(new ErrorResponse(400, "Currency code pair is missing."));

        var (baseCode, targetCode) = ParseCurrencyCodePair(codePair);
        var updatedExchangeRate = exchangeRateService.UpdateExchangeRate(
            new ExchangeRateFormDto(baseCode, targetCode, rate)
        );
        return Ok(updatedExchangeRate);
    }

    /// <summary>
    /// Extracts base and target codes from a currency pair code.
    /// </summary>
    /// <param name="codePair">Currency pair code (e.g. "USDEUR").</param>
    /// <returns>Base and target codes.</returns>
    private static (string, string) ParseCurrencyCodePair(string codePair) {
        return (
            codePair[..ValidCurrencyCodeAttribute.CodeLength],
            codePair[ValidCurrencyCodeAttribute.CodeLength..]
        );
    }
}