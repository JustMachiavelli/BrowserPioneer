using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace CrawlPioneer.Infrastructure.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {

            // 配置应用日志的 Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With<SimplifiedSourceContextEnricher>()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {SourceContext:-30} - {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(configuration["Logs:App:Path"]!,
                              rollingInterval: RollingInterval.Day,
                              outputTemplate: configuration["Logs:App:Template"]!,
                              restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(configuration["Logs:Error:Path"]!,
                              rollingInterval: RollingInterval.Day,
                              outputTemplate: configuration["Logs:Error:Template"]!,
                              restrictedToMinimumLevel: LogEventLevel.Warning)
                )
                .CreateLogger();

            hostBuilder.UseSerilog();

            return hostBuilder;
        }
    }

    internal class SimplifiedSourceContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue? sourceContext))
            {
                var sourceContextString = sourceContext.ToString().Trim('"');
                if (sourceContextString.Contains("."))
                {
                    var parts = sourceContextString.Split('.');
                    var simpleCategoryName = string.Join(".", parts.Select((part, index) => index < parts.Length - 1 ? part[0].ToString() : part));
                    var simplifiedProperty = new LogEventProperty("SourceContext", new ScalarValue(simpleCategoryName));
                    logEvent.AddOrUpdateProperty(simplifiedProperty);
                }
            }
        }
    }
}
