using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RacketsScrapper.Application
{
    public class ServiceDispatcher : IServiceDispatcher
    {
        private readonly ITennisPointScraperService _TennisPointService;
        private readonly string tennisPointUrl;

        public ServiceDispatcher(ITennisPointScraperService tennisPointScraperService)
        {
            _TennisPointService = tennisPointScraperService;
            tennisPointUrl = "https://www.tennis-point.it/padel-racchette-da-padel/?start=0&sz=36";
        }

        public void RunTennisPointScraper()
        {
            _TennisPointService.GetPageHtmlCode(tennisPointUrl);
            string? url = _TennisPointService.getNextPageLink();
            while (url != null)
            {
                Console.WriteLine("PAGINA: " + _TennisPointService.GetCurrentPage());
                _TennisPointService.ReadAllRacketsLinks();
                _TennisPointService.TakeRacketsData();
                _TennisPointService.GetPageHtmlCode(url);
                url = _TennisPointService.getNextPageLink();
                Console.WriteLine(">>>>NEXT PAGE LINK: " + url);
                _TennisPointService.CleanLinkList();
            }
        }
        public void RunPadelNuestroScraper()
        {
            throw new NotImplementedException();
        }

    }
}
