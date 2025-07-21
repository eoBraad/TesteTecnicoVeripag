using System.Text.Json;
using Application.Clients;
using Application.Dtos.Responses;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class GetWeatherForecastService(
    IGeoServiceClient geoServiceClient,
    IOpenWeatherClient openWeatherClient,
    ISearchHistoryRepository repository,
    ICachingRepository cachingRepository)
{
    private readonly IGeoServiceClient _geoServiceClient = geoServiceClient;
    private readonly IOpenWeatherClient _openWeatherClient = openWeatherClient;
    private readonly ISearchHistoryRepository _searchHistoryRepository = repository;
    private readonly ICachingRepository _cachingRepository = cachingRepository;

    public async Task<OpenWeatherSimpleResponse?> GetWeatherAsync(string location, string apiKey)
    {
        if (location == null)
            throw new AppValidationException(["Location cannot be null"]);
        
        var cachedResponse = await _cachingRepository.GetCachedResultAsync(location, SearchTypes.Simple);

        if (cachedResponse != null)
        {
            await _searchHistoryRepository.CreateSearchHistoryAsync(location, cachedResponse!, apiKey);
            return JsonSerializer.Deserialize<OpenWeatherSimpleResponse>(cachedResponse)!;
        }

        var result = await _geoServiceClient.GetGeoDataAsync(location);

        if (result == null)
        {
            throw new AppNotFoundException(["Location not found"]);
        }

        var weather = await _openWeatherClient.GetWeatherAsync(
            float.Parse(result.Lat.Substring(0, 6).Replace(".", ",")),
            float.Parse(result.Lon.Substring(0, 6).Replace(".", ",")));

        if (weather == null)
        {
            throw new AppNotFoundException(["Weather data not found"]);
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
        await _searchHistoryRepository.CreateSearchHistoryAsync(location, JsonSerializer.Serialize(response), apiKey);
        
        await _cachingRepository.CreateCachedResultAsync(location, JsonSerializer.Serialize(response), SearchTypes.Simple);

        return response;
    }
}