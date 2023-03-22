using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    public interface IDownloaderService
    {
        public Task<string> DownloadHtmlAsync(string targetUrl);
    }
}
