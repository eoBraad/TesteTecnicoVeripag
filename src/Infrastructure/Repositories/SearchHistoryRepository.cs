using Domain.Entity;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SearchHistoryRepository(AppDbContext context) : ISearchHistoryRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task CreateSearchHistoryAsync(string searchTerm, string result, string apiKey)
    {
        var searchHistory = new Domain.Entity.SearchHistory
        {
            Query = searchTerm,
            Result = result,
            ApiKey = apiKey
        };

        await _context.SearchHistories.AddAsync(searchHistory);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SearchHistory>> GetAllHistoryAsync(string apiKey)
    {
        return await context.SearchHistories.AsNoTracking().Where(sh => sh.ApiKey == apiKey)
            .ToListAsync();
    }
}