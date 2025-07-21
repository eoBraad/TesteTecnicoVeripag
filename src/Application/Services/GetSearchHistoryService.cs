using Domain.Entity;
using Domain.Repositories;

namespace Application.Services;

public class GetSearchHistoryService(ISearchHistoryRepository repository)
{
    public async Task<List<SearchHistory>> HandleAsync(string apiKey)
    {
        return await repository.GetAllHistoryAsync(apiKey);
    }
}