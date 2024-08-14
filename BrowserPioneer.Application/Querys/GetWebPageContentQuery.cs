using MediatR;

namespace CrawlerPioneer.Application.Querys
{
    public class GetWebPageContentQuery : IRequest<string>
    {
        public string PageUrl { get; set; } = default!;
    }
}
