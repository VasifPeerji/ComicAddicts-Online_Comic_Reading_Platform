using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSL.Models
{
    public class FreeTrials
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Trials { get; set;}
        public int Downloads { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

    }
}