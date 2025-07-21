using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class KeyRepository(AppDbContext context) : IKeyRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<bool> ValidateKey(string key)
    {
        return await _context.Keys.AnyAsync(k => k.ApiKey == key && k.ExpiresAt > DateTime.Now);
    }

    public async Task SaveKey(string key)
    {
        var apiKey = new Domain.Entity.Key()
        {
            ApiKey = key,
            ExpiresAt = DateTime.Now.AddYears(1) // Example expiration, adjust as needed
        };

        _context.Keys.Add(apiKey);
        
        await _context.SaveChangesAsync();
    }
}