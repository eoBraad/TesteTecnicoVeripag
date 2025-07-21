using Domain.Entity;

namespace Domain.Repositories;

public interface ICachingRepository
{
    public Task<string?> GetCachedResultAsync(string key, SearchTypes type);
    
    public Task CreateCachedResultAsync(string key, string result, SearchTypes type);
    
}