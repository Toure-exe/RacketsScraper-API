using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;
using RacketsScrapper.Domain;
using RacketsScrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RacketsScrapper.Application
{
    public class PadelNuestroScraperService : IRacketScraperService
    {

        private readonly List<string> links;
        public string CurruentPageCode { get; set; }
        private readonly IDownloaderService _downloaderService;
        private readonly IRacketsRepository _racketRepository;
        private int page;
        private string currentPageUrl;

        public PadelNuestroScraperService(IDownloaderService downloaderService, IRacketsRepository racketsRepository)
        {
            _downloaderService = downloaderService;
            _racketRepository = racketsRepository;
            links = new List<string>();
            page = 1;
            currentPageUrl = $"https://www.padelnuestro.com/it/racchette-padel-c-49.html?page=[PAGE_NUM]&sort=2a";
        }
        public void CleanLinkList()
        {
            this.links.Clear();
        }

        public int GetCurrentPage() => this.page;

        public string GetCurrentPageUrl() => this.currentPageUrl.Replace("[PAGE_NUM]",$"{this.page}");


        public List<string> getLinkList() => this.links;

        public string? getNextPageLink()
        {
             HtmlDocument doc = new HtmlDocument();
             doc.LoadHtml(CurruentPageCode);
             var next = doc.DocumentNode.SelectSingleNode("//*[@title=\" Pagina Successiva \"]");
             if(next != null)
            {
                this.page++;
                return this.currentPageUrl.Replace("[PAGE_NUM]", $"{this.page}");
            }
            else
            {
                return null;
            }
            

        }

        public void GetPageHtmlCode(string url)
        {
            CurruentPageCode = _downloaderService.DownloadHtmlAsync(url).Result;
        }

        public void ReadAllRacketsLinks()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(CurruentPageCode);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id=\"1-1\"]/strong/strong/strong/a[@href]");
            if(nodes == null)
            {
                nodes = doc.DocumentNode.SelectNodes("//*[@id=\"3\"]/strong/strong/strong/a[@href]");
            }

            foreach (HtmlNode linkNode in nodes)
            {
                HtmlAttribute attribute = linkNode.Attributes["href"];
                if(attribute != null && !attribute.Value.IsNullOrEmpty())
                {
                    links.Add(attribute.Value);
                }
                
            }
        }

        public void TakeRacketsData()
        {
            

            foreach (string url in this.links)
            {
                int i = 0;
                Racket racket = new Racket();
                racket.Url = url;
                string detailPage = _downloaderService.DownloadHtmlAsync(url).Result;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(detailPage);
                racket.Prezzo = double.Parse((doc.DocumentNode.SelectSingleNode("//*[@id=\"precio_articulo\"]/@content")).Attributes["content"].Value);
                racket.ImageLink = (doc.DocumentNode.SelectSingleNode("//*[@id=\"piGal\"]/ul[1]/li[1]/a/@href")).Attributes["href"].Value;
                racket.Marca = doc.DocumentNode.SelectSingleNode("//*[@id=\"bodyContent\"]/form/div/div/div/div[1]/ol/li[2]/div/div[2]/div[3]/h1/span").InnerText;            
                var titles = doc.DocumentNode.SelectNodes("//*[@id=\"bodyContent\"]/form/div/div/div/div[1]/ol/li[1]/div[2]/div/div[2]/table/tbody/tr/td[1]/b");
                var contents = doc.DocumentNode.SelectNodes("//*[@id=\"bodyContent\"]/form/div/div/div/div[1]/ol/li[1]/div[2]/div/div[2]/table/tbody/tr/td[2]/p");
                if(titles != null)
                {
                    foreach (HtmlNode nodes in titles)
                    {
                        switch (nodes.InnerText)
                        {
                            case "TIPO DI GIOCO: ":
                                racket.TipoDiGioco = contents.ElementAt(i).InnerText;
                                break;
                            case "TELAIO: ":
                                racket.Telaio = contents.ElementAt(i).InnerText;
                                break;
                            case "SESSO: ":
                                racket.Sesso = contents.ElementAt(i).InnerText;
                                break;
                            case "NUCLEO: ":
                                racket.Nucleo = contents.ElementAt(i).InnerText;
                                break;
                            case "LIVELLO DI GIOCO : ":
                                racket.LivelloDiGioco = contents.ElementAt(i).InnerText;
                                break;
                            case "FORMA: ":
                                racket.Forma = contents.ElementAt(i).InnerText;
                                break;
                            case "ETÀ: ":
                                racket.Eta = contents.ElementAt(i).InnerText;
                                break;
                            case "COLORE: ":
                                racket.ColoreUno = contents.ElementAt(i).InnerText;
                                break;
                            case "BILANCIAMENTO: ":
                                racket.Bilanciamento = contents.ElementAt(i).InnerText;
                                break;
                            case "ANNO: ":
                                racket.Anno = contents.ElementAt(i).InnerText;
                                break;
                            default: break;
                        }
                        i++;
                    }
                }
                
                Console.WriteLine($"--> {racket.Prezzo}, img: {racket.ImageLink}, tipo: {racket.TipoDiGioco}, sesso: {racket.Sesso}, colore :{racket.ColoreUno}, anno: {racket.Anno} \n\n");
                    _racketRepository.InsertRacket(racket);                  
            }
        }
    }
}
