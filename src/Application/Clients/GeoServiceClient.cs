using System.Text.Json;
using Application.Dtos.Results;

namespace Application.Clients;

public interface IGeoServiceClient
{
    Task<GeoResult?> GetGeoDataAsync(string searchTerm);
}

public class GeoServiceClient: IGeoServiceClient
{
    private readonly HttpClient _httpClient;
    public GeoServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "GeoApiApp");
    }
    
    public async Task<GeoResult?> GetGeoDataAsync(string searchTerm)
    {
        var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(searchTerm)}&format=json";

        var response = await _httpClient.GetStringAsync(url);

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var results = JsonSerializer.Deserialize<GeoResult[]>(response, options);

        return results != null && results.Length > 0 ? results[0] : null;
    }
}