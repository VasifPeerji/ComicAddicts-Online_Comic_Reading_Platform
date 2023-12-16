using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSL.Models
{
    public class Inventory
    {
            public int Id { get; set; }
            public Comics Comic { get;set; }
            public int ComicId { get; set; }
            public Merchandise Merchandise { get; set; } 
            public int MerchandiseId { get; set; } // Foreign key to link with Merchandise
            public int Quantity { get; set; }
        
    }
}