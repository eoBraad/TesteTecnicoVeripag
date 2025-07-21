using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class CreateApiKeyService(IKeyRepository keyRepository, IConfiguration configuration)
{
    private readonly IKeyRepository _keyRepository = keyRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> CreateApiKeyAsync(string secretKey)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new AppValidationException(["Secret key is required."]);
        
        var appSecretKey = _configuration["AppSecretKey"];

        if (appSecretKey != secretKey)
        {
            throw new AppUnauthorizedException(["Invalid secret key."]);
        }

        var apiKey = Guid.NewGuid().ToString();
        
        await _keyRepository.SaveKey(apiKey);
        
        return apiKey;
    }
}