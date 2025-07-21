using Application.Clients;
using Application.Dtos.Responses;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class GetWeatherForecastService(GeoServiceClient geoServiceClient, OpenWeatherClient openWeatherClient, ISearchHistoryRepository repository)
{
    private readonly GeoServiceClient _geoServiceClient = geoServiceClient;
    private readonly OpenWeatherClient _openWeatherClient = openWeatherClient;
    private readonly ISearchHistoryRepository _searchHistoryRepository = repository;

    public async Task<OpenWeatherSimpleResponse?> GetWeatherAsync(string location, string apiKey)
    {
        var result = await _geoServiceClient.GetGeoDataAsync(location);

        if (result == null)
        {
            throw new AppValidationException(["Location not found"]);
        }

        var weather = await _openWeatherClient.GetWeatherAsync(float.Parse(result.Lat.Substring(0, 6).Replace(".", ",")),
            float.Parse(result.Lon.Substring(0, 6).Replace(".", ",")));

        if (weather == null)
        {
            throw new AppValidationException(["Weather data not found"]);
        }

        var response = new OpenWeatherSimpleResponse
        {
            Weather = weather.Current.Weather,
            Temperature = weather.Current.Temp,
            FeelsLike = weather.Current.Feels_Like,
            WindSpeed = weather.Current.Wind_Speed,
            Humidity = weather.Current.Humidity,
        };
        
        // Save search history
        await _searchHistoryRepository.CreateSearchHistoryAsync(location, response.ToString()!, apiKey);

        return response;
    }
}