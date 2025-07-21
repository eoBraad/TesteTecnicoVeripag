using Application.Clients;
using Domain.Exceptions;

namespace Application.Services;

public class GetWeatherForecastService(GeoServiceClient geoServiceClient)
{
    private readonly GeoServiceClient _geoServiceClient = geoServiceClient;
    
    public async Task GetWeatherAsync(string location)
    {
        var result = await _geoServiceClient.GetGeoDataAsync(location);

        if (result == null)
        {
            throw new AppValidationException(["Location not found"]);
        }
        
        Console.WriteLine(result);
    }
}
