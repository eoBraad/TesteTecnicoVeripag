using System.Text.Json;
using Application.Clients;
using Application.Dtos.Responses;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class GetWeatherForecastExtendedService(
    IGeoServiceClient geoServiceClient,
    IOpenWeatherClient openWeatherClient,
    ISearchHistoryRepository repository,
    ICachingRepository cachingRepository)
{
    private readonly IGeoServiceClient _geoServiceClient = geoServiceClient;
    private readonly IOpenWeatherClient _openWeatherClient = openWeatherClient;
    private readonly ISearchHistoryRepository _searchHistoryRepository = repository;
    private readonly ICachingRepository _cachingRepository = cachingRepository;
    
    public async Task<OpenWeatherExtendedResponse> GetWeatherAsync(string location, string apiKey)
    {
        if (location == null)
        {
            throw new AppValidationException(["Location cannot be null"]);
        }
        
        var cachedResult = await _cachingRepository.GetCachedResultAsync(location, SearchTypes.Extended);

        if (cachedResult != null)
        {
            await _searchHistoryRepository.CreateSearchHistoryAsync(location, cachedResult!, apiKey);
            return JsonSerializer.Deserialize<OpenWeatherExtendedResponse>(cachedResult)!;
        }
        
        var result = await _geoServiceClient.GetGeoDataAsync(location);
        
        if (result == null)
        {
            throw new AppNotFoundException(["Location not found"]);
        }
        
        var weather = await _openWeatherClient.GetWeatherExtendedAsync(
            float.Parse(result.Lat.Substring(0, 6).Replace(".", ",")),
            float.Parse(result.Lon.Substring(0, 6).Replace(".", ","))
        );
        
        if (weather == null)
        {
            throw new AppNotFoundException(["Weather data not found"]);
        }

        await _searchHistoryRepository.CreateSearchHistoryAsync(location, weather.ToString()!, apiKey);
        
        var response = new OpenWeatherExtendedResponse();

        for (var i = 0; i < 5; i++)
        {
            response.Daily.Add(new OpenWeatherExtendedResponse.TempDays()
            {
                Dt = weather.Daily[i].Dt,
                Max = weather.Daily[i].Temp.max,
                Min = weather.Daily[i].Temp.min,
                Summary = weather.Daily[i].Summary,
                Temp = weather.Daily[i].Temp.Day
            });
        }
        
        await _cachingRepository.CreateCachedResultAsync(
            location.ToLowerInvariant(),
            JsonSerializer.Serialize(response),
            SearchTypes.Extended
        );
        
        return response;
    }
}
