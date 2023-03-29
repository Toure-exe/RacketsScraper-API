using Microsoft.Extensions.DependencyInjection;
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
        private readonly IRacketScraperService? _TennisPointService;
        private readonly IRacketScraperService? _PadelNuestroService;
        private readonly string tennisPointUrl;
        private readonly string padelNuestroUrl;
        private readonly IServiceProvider _serviceProvider;

        public ServiceDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            IEnumerable<IRacketScraperService> services = _serviceProvider.GetServices<IRacketScraperService>();
            _TennisPointService = services.FirstOrDefault(x => x.GetType() == typeof(TennisPointScraperService));
            _PadelNuestroService = services.FirstOrDefault(x => x.GetType() == typeof(PadelNuestroScraperService));
            tennisPointUrl = "https://www.tennis-point.it/padel-racchette-da-padel/?start=0&sz=36";
            padelNuestroUrl = "https://www.padelnuestro.com/it/racchette-padel-c-49.html";
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
            string? url = "";
            do
            {
                _PadelNuestroService.GetPageHtmlCode(_PadelNuestroService.GetCurrentPageUrl());
                _PadelNuestroService.ReadAllRacketsLinks();
                _PadelNuestroService.TakeRacketsData();
                url = _PadelNuestroService.getNextPageLink();
                Console.WriteLine("\n>>>>NEXT PAGE LINK: " + url + "\n");
                _PadelNuestroService.CleanLinkList();
            } while (url != null);
        }

    }
}
