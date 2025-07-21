namespace Application.Dtos.Results;

public class WeatherSimpleResult
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Timezone { get; set; }
    public CurrentWeather Current { get; set; }


    public class CurrentWeather
    {
        public long Dt { get; set; }
        public double Temp { get; set; }
        public double Feels_Like { get; set; }
        public double Wind_Speed { get; set; }
        public int Humidity { get; set; }
        public WeatherDescription[] Weather { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}