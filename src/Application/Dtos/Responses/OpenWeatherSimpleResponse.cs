using Application.Dtos.Results;

namespace Application.Dtos.Responses;

public class OpenWeatherSimpleResponse
{
    public WeatherSimpleResult.WeatherDescription[] Weather { get; set; }
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public double WindSpeed { get; set; }
    public int Humidity { get; set; }
}