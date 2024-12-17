using CurrencyExchange.Dto;
using CurrencyExchange.Exceptions;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("api")]
public class CurrenciesController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrenciesController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet("currencies")]
    public ActionResult<IEnumerable<CurrencyDto>> GetCurrencies()
    {
        try
        {
            var currencyDtos = _currencyService.GetAllCurrencies();
            return Ok(currencyDtos);
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("currency/{code?}")]
    public ActionResult<CurrencyDto> GetCurrency([ValidCurrencyCode] string? code)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest(new {message = "Currency code is missing."});
        try
        {
            var currency = _currencyService.GetCurrency(code!);
            if (currency == null)
                return NotFound(new { message = "Currency is not found" });
            return Ok(currency);
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("currencies")]
    public IActionResult PostCurrency([FromForm] CreateCurrencyDto createCurrencyDto)
    {
        try
        {
            var addedCurrency = _currencyService.AddCurrency(createCurrencyDto);
            return CreatedAtAction(
                nameof(GetCurrency), new { code = createCurrencyDto.Code }, addedCurrency);
        }
        catch (DuplicateDataException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}