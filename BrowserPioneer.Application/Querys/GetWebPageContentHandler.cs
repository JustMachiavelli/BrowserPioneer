using CrawlerPioneer.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrawlerPioneer.Application.Querys
{
    public class GetWebPageContentHandler(ILogger<GetWebPageContentHandler> logger, IWebpageService webpageService)
        : IRequestHandler<GetWebPageContentQuery, string>
    {
        public Task<string> Handle(GetWebPageContentQuery request, CancellationToken cancellationToken)
        {
            return webpageService.GetHtml(request.PageUrl);
        }
    }
}
