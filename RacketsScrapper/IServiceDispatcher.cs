using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Application
{
    public interface IServiceDispatcher
    {
        public void RunTennisPointScraper();

        public void RunPadelNuestroScraper();
    }
}
