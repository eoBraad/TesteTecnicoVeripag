namespace Api.Middleware;

using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private const string ApiKeyQueryName = "apikey";

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        if (!context.Request.Query.TryGetValue(ApiKeyQueryName, out var extractedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is missing.");
            return;
        }

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var apiKeyExists = await dbContext.Keys
            .AnyAsync(k => k.ApiKey == extractedApiKey.First());

        if (!apiKeyExists)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await next(context);
    }
}