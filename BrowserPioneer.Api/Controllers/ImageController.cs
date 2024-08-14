using CrawlPioneer.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrawlPioneer.Api.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController(ILogger<ImageController> logger, IMediator mediator) : ControllerBase
    {
        [HttpPost("download")]
        public async Task<IActionResult> FetchImage([FromBody] DownloadImageCommand command)
        {
            logger.LogInformation("准备下载图片");
            byte[] result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
