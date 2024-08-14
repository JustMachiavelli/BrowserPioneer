using CrawlerPioneer.Application.Interfaces;
using CrawlPioneer.Infrastructure.Helpers;

namespace CrawlerPioneer.Infrastructure.Service
{
    public class WebpageService(SeleniumRequester seleniumRequester) : IWebpageService
    {
        public async Task<string> GetHtml(string url)
        {
            return await seleniumRequester.GetHtml(url);
        }
    }
}
