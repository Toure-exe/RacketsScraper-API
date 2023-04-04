using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Domain
{
    public class ResponseObjectHome : ResponseObject
    {
        public IEnumerable<string> Marche { get; set; }

        public IEnumerable<string> Colori { get; set; }

        public IEnumerable<string> Sessi { get; set; }
    }
}
