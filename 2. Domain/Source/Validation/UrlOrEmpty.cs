using System.ComponentModel.DataAnnotations;

public class UrlOrEmptyAttribute : ValidationAttribute
{
    #pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    #pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success!;
        }

        var emailAttribute = new UrlAttribute();
        if (!emailAttribute.IsValid(value))
        {
            return new ValidationResult("The url is not valid.");
        }

        return ValidationResult.Success!;
    }
}
