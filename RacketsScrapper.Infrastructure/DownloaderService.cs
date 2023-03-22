using System;

namespace RacketsScrapper.Infrastructure
{
    public class DownloaderService : IDownloaderService
    {
        public string Url { get; set; }

        public async Task<string> DownloadHtmlAsync(string targetUrl)
        {
            string result = "";


            using (HttpClient client = new HttpClient())
            {
                using HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(targetUrl, UriKind.Absolute);
                using HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content != null)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            Url = targetUrl;
            return result;
        }
    }
}