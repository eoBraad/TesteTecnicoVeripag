namespace Domain.Repositories;

public interface ISearchHistoryRepository
{
    void CreateSearchHistoryAsync(string searchTerm, string result, string apiKey);
}