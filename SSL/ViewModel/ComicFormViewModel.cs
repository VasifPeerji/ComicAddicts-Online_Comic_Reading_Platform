using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSL.Models;

namespace SSL.ViewModel
{
    public class ComicFormViewModel
    {
        public Comics Comic { get; set; }
        public List<GenreDropdown> GenresDropDown { get; set; }

        public Ratings Rating { get; set; }
    }
}