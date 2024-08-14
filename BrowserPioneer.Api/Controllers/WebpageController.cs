using CrawlerPioneer.Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrawlerPioneer.Api.Controllers
{
    [Route("api/webpage")]
    [ApiController]
    public class WebpageController(ILogger<WebpageController> logger, IMediator mediator) : ControllerBase
    {
        [HttpGet("content")]
        public async Task<IActionResult> GetWebPageContent([FromQuery] string pageUrl)
        {
            GetWebPageContentQuery query = new GetWebPageContentQuery
            {
                PageUrl = pageUrl
            };

            var htmlContent = await mediator.Send(query);

            // Return the HTML as a string
            return Content(htmlContent, "text/html");
        }
    }
}
