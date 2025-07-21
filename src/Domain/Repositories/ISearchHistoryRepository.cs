namespace Domain.Repositories;

public interface ISearchHistoryRepository
{
    Task CreateSearchHistoryAsync(string searchTerm, string result, string apiKey);
}