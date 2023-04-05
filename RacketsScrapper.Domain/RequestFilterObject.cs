using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Domain
{
    public class RequestFilterObject
    {

        public string Order { get; set; }

        public List<string> Colors { get; set; }

        public List<string> SexList { get; set; }

        public List<string> Brands { get; set; }
    }
}
