using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SSL.Models
{
    public class RemoveIfExistsValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = new ApplicationDbContext(); // Create an instance of your data context.
            var comics = (Comics)validationContext.ObjectInstance;
            var existingProduct = dbContext.Comics.FirstOrDefault(c => c.ReadOnline == comics.ReadOnline && c.Id != comics.Id);

            if (existingProduct != null)
            {
                dbContext.Comics.Remove(existingProduct);
                dbContext.SaveChanges();
               /* return new ValidationResult("Comic Already Exists !");*/
            }

            return ValidationResult.Success;
        }
    }
}