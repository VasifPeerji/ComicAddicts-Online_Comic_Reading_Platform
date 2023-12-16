using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSL.Models
{
    public class Ratings
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? ComicId { get; set; }
        public int? MerchandiseId { get; set; }
        public int? Stars { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Comics Comic { get; set; }
        public virtual Merchandise Merchandise { get; set; }
    }
}