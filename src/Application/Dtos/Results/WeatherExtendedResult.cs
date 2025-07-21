namespace Application.Dtos.Results;

public class WeatherExtendedResult
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Timezone { get; set; }
    public List<DailyWeather> Daily { get; set; }


    public class DailyWeather
    {
        public long Dt { get; set; }
        public Temp Temp { get; set; }
        public string Summary { get; set; }
    }
    
    public class Temp
    {
        public double Day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }
}