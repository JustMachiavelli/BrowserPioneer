using CrawlPioneer.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrawlPioneer.Application.Commands
{
    public class DownloadImageHandler(ILogger<DownloadImageHandler> logger, IImageService imageService) 
        : IRequestHandler<DownloadImageCommand, byte[]>
    {
        public async Task<byte[]> Handle(DownloadImageCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("获取【{PageUrl}】网页位置【{ImageXPath}】图片", request.PageUrl, request.ImageXPath);

            return await imageService.FetchImage(
                request.PageUrl,
                request.ImageXPath,
                request.ObstacleXPath,
                request.DownCount,
                request.ImageName
            );
        }
    }
}
