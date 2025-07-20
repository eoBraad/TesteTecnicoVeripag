namespace Domain.Entity;

public class Key : EntityBase
{
    public string ApiKey { get; set; } = string.Empty;
    
    public DateTime ExpiresAt { get; set; }
}