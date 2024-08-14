using CrawlPioneer.Application.Interfaces;
using CrawlPioneer.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CrawlPioneer.Infrastructure.Service
{
    internal class ImageService(ILogger<ImageService> logger, SeleniumRequester requester, IConfiguration configuration) : IImageService
    {
        public async Task<byte[]> FetchImage(string pageUrl, string imageXPath, string? obstacleXPath, byte downCount, string imageName)
        {
            string savePath = Path.Combine(configuration["DownloadDirectory:Default"]!, imageName);
            logger.LogInformation("下载图片至【{savePath}】", savePath);

            await requester.DownloadImageAsync(
                pageUrl,
                imageXPath,
                obstacleXPath,
                downCount,
                savePath
            );
            if (!File.Exists(savePath))
            {
                throw new FileNotFoundException($"下载图片后，【{savePath}】仍然不存在");
            }
            return await File.ReadAllBytesAsync(savePath);
        }
    }
}
