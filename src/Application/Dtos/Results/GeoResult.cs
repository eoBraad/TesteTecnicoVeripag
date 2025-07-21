namespace Application.Dtos.Results;

public class GeoResult
{
    public string Lat { get; set; } 
    public string Lon { get; set; }
    public string Display_Name { get; set; } = string.Empty;
}