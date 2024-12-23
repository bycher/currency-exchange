using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.Api.Validation;

/// <summary>
/// Custom validation attribute to check if the value is not equal to another property.
/// </summary>
/// <param name="otherProperty">The name of the other property to compare with.</param>
public class NotEqualToAttribute(string otherProperty) : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
        if (otherPropertyInfo == null)
            return new ValidationResult($"Property '{otherProperty}' not found.");

        var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
        return Equals(value, otherValue)
            ? new ValidationResult($"The value must not be equal to '{otherProperty}'.")
            : ValidationResult.Success;
    }
}
