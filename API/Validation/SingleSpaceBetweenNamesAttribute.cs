using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.Validation
{
    public class SingleSpaceBetweenNamesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // Validation passed if the value is null (you can change this behavior if needed)
                return ValidationResult.Success;
            }

            string userName = value.ToString();
            // Use a regular expression to check for consecutive spaces
            if (Regex.IsMatch(userName, @"\s{2,}"))
            {
                return new ValidationResult("User name should have only one space between names.");
            }

            return ValidationResult.Success;
        }
    }
    //public static class SingleSpaceBetweenNamesAttribute
    //{
    //	public static bool SpaceValidation(object value)
    //	{
    //		if (value == null)
    //		{
    //			// Validation passed if the value is null (you can change this behavior if needed)
    //			return false;
    //		}

    //		string userName = value.ToString();
    //		// Use a regular expression to check for consecutive spaces
    //		if (Regex.IsMatch(userName, @"\s{2,}"))
    //		{
    //			return false;
    //		}

    //		return true;
    //	}
    //}
}
