using Application.Clients;
using Application.Dtos.Responses;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class GetWeatherForecastExtendedService(
    GeoServiceClient geoServiceClient,
    OpenWeatherClient openWeatherClient,
    ISearchHistoryRepository repository)
{
    private readonly GeoServiceClient _geoServiceClient = geoServiceClient;
    private readonly OpenWeatherClient _openWeatherClient = openWeatherClient;
    private readonly ISearchHistoryRepository _searchHistoryRepository = repository;
    
    public async Task<OpenWeatherExtendedResponse> GetWeatherAsync(string location, string apiKey)
    {
        if (location == null)
        {
            throw new AppValidationException(["Location cannot be null"]);
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
        
        return response;
    }
}
