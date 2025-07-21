using System.Globalization;
using System.Text.Json;
using Application.Dtos.Results;

namespace Application.Clients;

public interface IOpenWeatherClient
{
    Task<WeatherSimpleResult?> GetWeatherAsync(float lat, float lon, string exclude = "minutely");
    Task<WeatherExtendedResult?> GetWeatherExtendedAsync(float lat, float lon, string exclude = "minutely");
}

public class OpenWeatherClient(HttpClient httpClient, string apiKey) : IOpenWeatherClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = apiKey;
    
    public async Task<WeatherSimpleResult?> GetWeatherAsync(float lat, float lon, string exclude = "minutely")
    {
        var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lon.ToString(CultureInfo.InvariantCulture)}&exclude={exclude}&appid={_apiKey}&units=metric&lang=pt_br";

        var response = await _httpClient.GetStringAsync(url);

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<WeatherSimpleResult>(response, options);
    }
    
    public async Task<WeatherExtendedResult?> GetWeatherExtendedAsync(float lat, float lon, string exclude = "minutely")
    {
        var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lon.ToString(CultureInfo.InvariantCulture)}&exclude={exclude}&appid={_apiKey}&units=metric&lang=pt_br";

        var response = await _httpClient.GetStringAsync(url);
        
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<WeatherExtendedResult>(response, options);
    }
}