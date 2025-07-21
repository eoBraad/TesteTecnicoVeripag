using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GetWeatherForecastService>();
        services.AddScoped<GetWeatherForecastExtendedService>();
        services.AddScoped<CreateApiKeyService>();
    }
}