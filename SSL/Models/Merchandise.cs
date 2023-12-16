using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SSL.Models
{
    [Table("Merchandise")]
    public class Merchandise
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide a description.")]
        [MaxLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string Description { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please enter a price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public Category Category { get; set; }
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please upload a photo of the product")]
        public string Image { get; set; }

 
    }
}