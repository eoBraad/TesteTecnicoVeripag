using Api.Filter;
using Application;
using Application.Clients;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure lowercase Routes
builder.Services.AddRouting(r => r.LowercaseUrls = true);
builder.Services.AddControllers();

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Configure Documentation
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Dependency Injection for Application layer
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Add Clients
builder.Services.AddHttpClient<GeoServiceClient>();

// Add Exception Filter
builder.Services.AddMvc(c => c.Filters.Add<AppExceptionFilter>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();