using HtmlAgilityPack;
using RacketsScrapper.Domain;
using RacketsScrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace RacketsScrapper.Application
{
    public class TennisPointScraperService : IRacketScraperService
    {
        /*
         *  tennis-point ha cambiato la struttura del proprio codice HTML (id, div, ecc...), per cui tutte le XPATH 
         *  vanno aggiornate
         * 
         */
        private readonly List<string> links;
        public string CurruentPageCode { get; set; }
        private readonly IDownloaderService _downloaderService;
        private readonly IRacketsRepository _racketRepository;
        private int page;
        public bool NextPage { get; set; }

        public TennisPointScraperService(IDownloaderService downloaderService, IRacketsRepository racketsRepository, ICacheService cacheService)
        {
            _downloaderService = downloaderService;
            _racketRepository = racketsRepository;
            links = new List<string>();
            NextPage = true;
            page = 0;
            CurruentPageCode = string.Empty;
        }

        public List<string> getLinkList() => this.links;

        public int GetCurrentPage() => this.page;

        public void CleanLinkList() => this.links.Clear();


        public void ReadAllRacketsLinks()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(CurruentPageCode);

            foreach (HtmlNode linkNode in
                doc.DocumentNode.SelectNodes("//*[@id=\"productList\"]/div/div/div[2]/div[2]/div[3]/a[@href]")) 
            {
                HtmlAttribute attribute = linkNode.Attributes["href"];
                links.Add("https://www.tennis-point.it"+attribute.Value);
            }
        }

        public void TakeRacketsData()
        {
            string price = "";
           
            foreach(string url in this.links)
            {
                Console.WriteLine("CURRENT URL: "+ url);
                Racket racket = new Racket();
                racket.Url = url;
                string detailPage = _downloaderService.DownloadHtmlAsync(url).Result;
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(detailPage);
                HtmlNode detailNode = doc.DocumentNode.SelectSingleNode("//*[@itemprop=\"image\"]/@href");
                HtmlAttribute attribute = detailNode.Attributes["href"];
                racket.ImageLink = attribute.Value;
                detailNode = doc.DocumentNode.SelectSingleNode("//*[@itemprop=\"price\"]");
                if (detailNode == null)
                    detailNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"js-pdp-redesign\"]/div[2]/div[2]/div/div/div[3]/div/div/div[1]/span");
                price = detailNode.InnerText;
                racket.Prezzo = double.Parse(price.Replace("&euro;", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture);

               detailNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"strike-through old-price list\"]/span");
               if(detailNode != null)
                    racket.VecchioPrezzo = double.Parse(detailNode.InnerText.Replace("&euro;", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture);

                detailNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"js-article-no\"]"); 
                if(detailNode != null)
                    racket.NumeroArticolo = detailNode.InnerText;

                detailNode = doc.DocumentNode.SelectSingleNode("//*[@itemprop=\"brand\"]/span");
                racket.Marca = detailNode.InnerText;
                var tempModello = doc.DocumentNode.SelectNodes("//*[@itemprop=\"name\"]");
                racket.Modello = tempModello.ElementAt(tempModello.Count - 1).InnerText;
                detailNode = doc.DocumentNode.SelectSingleNode("/html/head/meta[6]/@content");
                var description = detailNode.Attributes["content"].Value.Split("-");
                foreach(string desc in description)
                {
                    if (desc.Contains("colore"))
                    {
                        var value = desc.Split(":");
                        racket.ColoreUno = value[1].Trim();
                    }
                    else if (desc.Contains("Profilo"))
                    {
                        var value = desc.Split(":");
                        racket.Profilo = int.Parse(value[1].Trim(), CultureInfo.InvariantCulture);
                    }
                    else if (desc.Contains("Peso"))
                    {
                        var value = desc.Split(":");
                        racket.Peso = value[1].Trim();
                    }
                    else if (desc.Contains("Unisex") || desc.Contains("Uomini") || desc.Contains("Donna") || desc.Contains("Bambini"))
                    {
                        racket.Sesso = desc.Trim();
                    }
                }
                //
                Console.WriteLine($"--> {racket.Prezzo}, img: {racket.ImageLink}, tipo: {racket.TipoDiProdotto}, num: {racket.NumeroArticolo}, colore 1:{racket.ColoreUno}, peso: {racket.Peso} \n\n");
                _racketRepository.InsertRacket(racket);

            }
        }

        public string? getNextPageLink()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(CurruentPageCode);
            HtmlNode next;
            if (page == 1)
            {
                next = doc.DocumentNode.SelectSingleNode("//*[@id=\"productList\"]/div[37]/div/div[2]/nav/ul/li[6]/a/@href");
            }
            else
            {
                next = doc.DocumentNode.SelectSingleNode("//*[@id=\"productList\"]/div[37]/div/div[2]/nav/ul/li[7]/a/@href");
            }
           
            if (next != null)
            {
                return (next.Attributes["href"]).Value;
            }
            else 
            {
                return null;
            }
        }

        public void GetPageHtmlCode(string url)
        {
            page++;
            CurruentPageCode = _downloaderService.DownloadHtmlAsync(url).Result;
        }

        public string GetCurrentPageUrl()
        {
            throw new NotImplementedException();
        }
    }
}
