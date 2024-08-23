using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class EmailAttribute : ValidationAttribute
{
    public string ErrorMessage;
    private static readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success ?? new ValidationResult("Unknown error occurred."); 
        }

        string? stringValue = value?.ToString();

        if (stringValue != null && _emailRegex.IsMatch(stringValue))
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult(ErrorMessage);
    }

}




