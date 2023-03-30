using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Domain
{
    public class ResponseObject
    {
        // inserire un attributo per il numero tot di articoli trovati
        public IEnumerable<Racket> ? Rackets { get; set; }

        public double Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}
