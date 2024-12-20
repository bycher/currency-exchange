using System.Net;
using CurrencyExchange.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyExchange.Validation;

/// <summary>
/// Filter to validate models from requests.
/// Short-circuits request if model state is invalid with a ValidationErrorResponse.
/// </summary>
public class ValidateModelFilter : IActionFilter {
    public void OnActionExecuting(ActionExecutingContext context) {
        if (!context.ModelState.IsValid) {
            var errors = context.ModelState.Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? []
                );
            var response = new ValidationErrorResponse(errors);
            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) {
    }
}
