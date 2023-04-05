using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Domain
{
    public class ResponseFilterObject
    {
        public IEnumerable<Racket> ? Rackets { get; set; }
    
        public double Pages { get; set; }

        public int CurrentPage { get; set; }

        public int Elements { get; set; }
    }
}
