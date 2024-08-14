
namespace CrawlPioneer.Application.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> FetchImage(string pageUrl, string imageXPath, string? obstacleXPath, byte downCount, string imageName);
    }
}
