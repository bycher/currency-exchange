using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CurrencyExchange.Validation;

/// <summary>
/// Custom validation attribute to ensure a currency code pair is valid.
/// </summary>
public partial class ValidCurrencyCodePairAttribute : ValidationAttribute {
    /// <summary>
    /// Regex to validate currency code pair format.
    /// </summary>
    private static readonly Regex _regex = new(@"^[A-Z]{6}$", RegexOptions.Compiled);

    protected override ValidationResult? IsValid(object? value, ValidationContext _) {
        if (value == null)
            return new ValidationResult("The currency code pair is missing.");

        if (!_regex.IsMatch(value.ToString()!))
            return new ValidationResult("Invalid currency code pair format.");

        return ValidationResult.Success;
    }
}