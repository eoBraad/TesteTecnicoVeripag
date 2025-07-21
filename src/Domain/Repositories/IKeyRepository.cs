namespace Domain.Repositories;

public interface IKeyRepository
{
    Task<bool> ValidateKey(string key);
    
    Task SaveKey(string key);
}