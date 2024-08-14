using CrawlPioneer.Application.Extensions;
using CrawlPioneer.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

//日志
builder.Host.UseSerilogLogging(builder.Configuration);

//基础设施
builder.Services.AddInfrastructure();

//业务应用
builder.Services.AddApplication();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
