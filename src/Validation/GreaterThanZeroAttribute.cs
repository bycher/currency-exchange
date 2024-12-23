using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CurrencyExchange.Api.Validation;

/// <summary>
/// Custom validation attribute to ensure a value is greater than zero.
/// Used to validate amount, exchange rate from request.
/// </summary>
public partial class GreaterThanZeroAttribute : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        if (value == null)
            return new ValidationResult("The value is missing.");

        if (!decimal.TryParse(
            value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var valueAsDecimal)
        )
            return new ValidationResult("The value must be a decimal.");

        if (valueAsDecimal <= 0)
            return new ValidationResult("The number must be greater than 0.");

        return ValidationResult.Success;
    }
}