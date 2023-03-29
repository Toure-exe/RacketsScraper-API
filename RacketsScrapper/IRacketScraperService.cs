using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Application
{
    public interface IRacketScraperService
    {
        public void ReadAllRacketsLinks();
        public void TakeRacketsData();

        public void GetPageHtmlCode(string url);
        public string? getNextPageLink();

        public List<string> getLinkList();

        public int GetCurrentPage();

        public string GetCurrentPageUrl();

        public void CleanLinkList();

    }
}
