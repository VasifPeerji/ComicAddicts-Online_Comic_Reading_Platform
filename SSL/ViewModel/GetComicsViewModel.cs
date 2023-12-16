using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSL.ViewModel
{
    public class GetComicsViewModel
    {
        public IEnumerable<IGrouping<string, SSL.Models.Comics>> Comics { get; set; }
        public Dictionary<int?, float> stars { get; set; }
    }
}