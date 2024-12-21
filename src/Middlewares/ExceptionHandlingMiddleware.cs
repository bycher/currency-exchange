using CurrencyExchange.Api.Exceptions;
using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Middlewares;

/// <summary>
/// Middleware component for handling all application exceptions.
/// Maps exceptions to HTTP status codes and serializes error responses to JSON.
/// </summary>
/// <param name="next">Next middleware in the pipeline.</param>
public class ExceptionHandlingMiddleware(RequestDelegate next) {
    /// <summary>
    /// Invoke middleware.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    public async Task InvokeAsync(HttpContext context) {
        try {
            await next(context);
        }
        catch (Exception ex) {
            var statusCode = ex switch {
                ResourceAlreadyExistsException => 409,
                ResourceNotFoundException => 404,
                _ => 500,
            };
            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse(
                statusCode, ex.Message, ex.InnerException?.Message
            );
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}