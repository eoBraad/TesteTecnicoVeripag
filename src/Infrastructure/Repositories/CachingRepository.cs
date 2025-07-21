using Domain;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CachingRepository(AppDbContext context) : ICachingRepository
{
    private readonly AppDbContext _dbContext = context;
    public async Task<string?> GetCachedResultAsync(string key, SearchTypes type)
    {
        var caching = await _dbContext.Cachings.FirstOrDefaultAsync(c => c.Location == key && c.Type == type);

        return caching?.Result ?? null;
    }

    public async Task CreateCachedResultAsync(string key, string result, SearchTypes type)
    {
        var caching = new Domain.Entity.Caching
        {
            Location = key,
            Result = result,
            Type = type
        };
        
        _dbContext.Cachings.Add(caching);

        await _dbContext.SaveChangesAsync();
    }
}