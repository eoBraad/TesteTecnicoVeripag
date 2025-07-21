using Domain.Repositories;
using Infrastructure.Database;

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
}