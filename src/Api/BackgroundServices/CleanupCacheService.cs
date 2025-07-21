using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BackgroundServices;

public class CleanupCacheService(ILogger<CleanupCacheService> logger, IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("CleanupService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    logger.LogInformation("Limpando tabela às {time}", DateTime.Now);

                    await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Cachings");

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao limpar a tabela.");
            }

            await Task.Delay(_interval, stoppingToken); // Aguarda 1 hora
        }
    }
}