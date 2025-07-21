namespace Application.Dtos.Responses;

public class OpenWeatherExtendedResponse
{
    public List<TempDays> Daily { get; set; } = [];

    public class TempDays
    {
        public long Dt { get; set; }
        
        public double Temp { get; set; }
        
        public string Summary { get; set; }
        
        public double Min { get; set; }
        
        public double Max { get; set; }
    }
}