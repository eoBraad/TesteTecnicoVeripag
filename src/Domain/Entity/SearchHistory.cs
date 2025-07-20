namespace Domain.Entity;

public class SearchHistory : EntityBase
{
    public string Query { get; set; } = string.Empty;
    
    public string Result { get; set; } = string.Empty;
    
    public string ApiKey { get; set; } = string.Empty;
}