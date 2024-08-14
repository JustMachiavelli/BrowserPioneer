using CrawlPioneer.Application.Extensions;
using CrawlPioneer.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

//��־
builder.Host.UseSerilogLogging(builder.Configuration);

//������ʩ
builder.Services.AddInfrastructure();

//ҵ��Ӧ��
builder.Services.AddApplication();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
