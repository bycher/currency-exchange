using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CurrencyExchange.Api.Validation;

/// <summary>
/// Custom validation attribute to ensure a currency code follows ISO 4217 format.
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

    protected override ValidationResult? IsValid(object? value, ValidationContext _) {
        if (value == null)
            return new ValidationResult("The currency code is missing.");

        if (!_regex.IsMatch(value.ToString()!))
            return new ValidationResult("Invalid currency code format (ISO 4217).");

        return ValidationResult.Success;
    }
}