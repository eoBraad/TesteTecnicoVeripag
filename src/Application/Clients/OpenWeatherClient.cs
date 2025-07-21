using System.Globalization;
using System.Text.Json;
using Application.Dtos.Results;

namespace Application.Clients;

public class OpenWeatherClient(HttpClient httpClient, string apiKey)
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
}