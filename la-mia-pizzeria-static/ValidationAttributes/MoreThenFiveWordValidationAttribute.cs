using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationAttributes
{
    public class MoreThenFiveWordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is string)
            {
                string input = (string)value;

                if(input == null || input.Split(' ').Length <= 5) 
                {
                    return new ValidationResult("La descrizione deve contenere almeno 5 parole");
                }

                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("L'input inserito non è una stringa");
            }
        }
    }
}
