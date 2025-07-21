namespace Domain.Entity;

public class Caching : EntityBase
{
    public SearchTypes Type { get; set; }

    public string Location { get; set; } = string.Empty;
    
    public string Result { get; set; } = string.Empty;
}