using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CurrencyExchange.Api.Validation;

/// <summary>
/// Custom validation attribute to ensure a currency code follows ISO 4217 format.
/// Also ensures the currency code is not null.
/// </summary>
public partial class ValidCurrencyCodeAttribute : ValidationAttribute {
    /// <summary>
    /// Length of the currency code.
    /// </summary>
    public const int CodeLength = 3;

    /// <summary>
    /// Regex to validate currency code format.
    /// </summary>
    private static readonly Regex _regex = new(
        @"^[A-Z]{" + CodeLength.ToString() + "}$", RegexOptions.Compiled);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        if (value == null)
            // Null values are handled by the Required attribute. This is just a fallback.
            return ValidationResult.Success;

        if (!_regex.IsMatch(value.ToString()!))
            return new ValidationResult("Invalid currency code format (ISO 4217).");

        return ValidationResult.Success;
    }
}