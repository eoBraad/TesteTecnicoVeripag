using System.Net;
using Api.Filter;
using Api.Middleware;
using Application;
using Application.Clients;
using Infrastructure;
using Infrastructure.BackgroundServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure lowercase Routes
builder.Services.AddRouting(r => r.LowercaseUrls = true);
builder.Services.AddControllers();

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Configure Documentation
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Dependency Injection for Application layer
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Add Clients
builder.Services.AddHttpClient<GeoServiceClient>();
builder.Services.AddHttpClient("OpenWeatherClient", client => { }).AddTypedClient(httpClient =>
    new OpenWeatherClient(httpClient, builder.Configuration["OpenWeatherApiKey"]!)).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };
});

// Add Background Services
builder.Services.AddHostedService<CleanupCacheService>();

builder.Services.AddMvc(c => c.Filters.Add<AppExceptionFilter>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/weatherforecast"), branch =>
{
    branch.UseMiddleware<ApiKeyMiddleware>();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();