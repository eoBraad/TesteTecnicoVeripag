using System.Text.Json;
using Application.Dtos.Results;

namespace Application.Clients;

public class GeoServiceClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    
    public async Task<GeoResult?> GetGeoDataAsync(string searchTerm)
    {
        var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(searchTerm)}&format=json";

        var response = await _httpClient.GetStringAsync(url);

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var results = JsonSerializer.Deserialize<GeoResult[]>(response, options);

        return results != null && results.Length > 0 ? results[0] : null;
    }
}