using CrawlerPioneer.Application.Interfaces;
using CrawlerPioneer.Infrastructure.Service;
using CrawlPioneer.Application.Interfaces;
using CrawlPioneer.Infrastructure.Helpers;
using CrawlPioneer.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CrawlPioneer.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<SeleniumRequester>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IWebpageService, WebpageService>();
        }
    }
}
