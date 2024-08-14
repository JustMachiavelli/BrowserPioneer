using MediatR;

namespace CrawlPioneer.Application.Commands
{
    public class DownloadImageCommand : IRequest<byte[]>
    {
        public string PageUrl { get; set; } = default!;

        public string ImageXPath { get; set; } = default!;

        public string? ObstacleXPath { get; set; }

        public byte DownCount { get; set; }

        public string ImageName { get; set; } = "default.jpg";
    }
}
