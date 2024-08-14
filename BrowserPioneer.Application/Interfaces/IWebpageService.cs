namespace CrawlerPioneer.Application.Interfaces
{
    public interface IWebpageService
    {
        public Task<string> GetHtml(string url);
    }
}
