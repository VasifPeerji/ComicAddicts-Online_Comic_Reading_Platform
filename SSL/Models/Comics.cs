using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSL.Models
{
    public class Comics
    {
        public int Id { get; set; }
      
        [Required]
       /* [StringLength(120, ErrorMessage ="{0} can have a max of {1} characters")]*/
        public string Summary { get; set; }
        public string Genre { get; set; }
        [Required]
        [Range(1, 50, ErrorMessage = "Pages should'nt be more than 50")]
        public int Pages { get; set; }
        [DateInThePast]
        public DateTime DateAdded { get; set; }
        [Required]
        public string Publisher { get; set; }
        [RemoveIfExistsValidation]
        public string ReadOnline { get; set; }

        [Required]
        [UniqueTitleValidation]
        public string Name { get; set; }

        public string Download { get; set; }
        public GenreDropdown GenreDropDown { get; set; }
        public int? GenreDropDownId { get; set; }
    }
}