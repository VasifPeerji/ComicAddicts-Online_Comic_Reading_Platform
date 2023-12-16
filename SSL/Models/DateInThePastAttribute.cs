using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSL.Models
{
    public class DateInThePastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (date > DateTime.Now)
            {
                return new ValidationResult("Release date must be in the past.");
            }

            return ValidationResult.Success;
        }
    }
}