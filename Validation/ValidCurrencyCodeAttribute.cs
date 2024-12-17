using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CurrencyExchange.Validation;

public partial class ValidCurrencyCodeAttribute : ValidationAttribute
{
    public const int CodeLength = 3;
    private static readonly Regex _regex = new(
        @"^[A-Z]{" + CodeLength.ToString() + "}$", RegexOptions.Compiled);

    protected override ValidationResult? IsValid(object? value, ValidationContext _)
    {
        if (value == null || !_regex.IsMatch(value.ToString()!))
            return new ValidationResult("Invalid currency code format (ISO 4217).");

        return ValidationResult.Success;
    }
}