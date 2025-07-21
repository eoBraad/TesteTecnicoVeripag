using System.Text.Json;
using Application.Clients;
using Application.Services;
using Application.Dtos.Responses;
using Application.Dtos.Results;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using Xunit;
using FluentAssertions;

public class GetWeatherForecastServiceTests
{
    private readonly Mock<IGeoServiceClient> _geoServiceMock = new();
    private readonly Mock<IOpenWeatherClient> _weatherClientMock = new();
    private readonly Mock<ISearchHistoryRepository> _searchHistoryMock = new();
    private readonly Mock<ICachingRepository> _cacheMock = new();

    private GetWeatherForecastService CreateService() =>
        new GetWeatherForecastService(
            _geoServiceMock.Object,
            _weatherClientMock.Object,
            _searchHistoryMock.Object,
            _cacheMock.Object);

    [Fact]
    public async Task Should_Return_From_Cache_When_Exists()
    {
        // Arrange
        var location = "São Paulo";
        var apiKey = "123";
        var cachedResponse = JsonSerializer.Serialize(new OpenWeatherSimpleResponse { Temperature = 25 });

        _cacheMock.Setup(c => c.GetCachedResultAsync(location, SearchTypes.Simple))
            .ReturnsAsync(cachedResponse);

        _searchHistoryMock.Setup(r => r.CreateSearchHistoryAsync(location, cachedResponse, apiKey))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var result = await service.GetWeatherAsync(location, apiKey);

        // Assert
        result.Should().NotBeNull();
        result.Temperature.Should().Be(25);
    }

    [Fact]
    public async Task Should_Throw_When_Location_Is_Null()
    {
        var service = CreateService();

        Func<Task> act = async () => await service.GetWeatherAsync(null, "123");

        await act.Should().ThrowAsync<AppValidationException>();
    }

    [Fact]
    public async Task Should_Throw_When_GeoData_Not_Found()
    {
        _cacheMock.Setup(c => c.GetCachedResultAsync(It.IsAny<string>(), It.IsAny<SearchTypes>()))
            .ReturnsAsync((string?)null);

        _geoServiceMock.Setup(g => g.GetGeoDataAsync(It.IsAny<string>()))
            .ReturnsAsync((GeoResult?)null);

        var service = CreateService();

        Func<Task> act = async () => await service.GetWeatherAsync("Unknown", "123");

        await act.Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task Should_Throw_When_Weather_Not_Found()
    {
        _cacheMock.Setup(c => c.GetCachedResultAsync(It.IsAny<string>(), It.IsAny<SearchTypes>()))
            .ReturnsAsync((string?)null);

        _geoServiceMock.Setup(g => g.GetGeoDataAsync(It.IsAny<string>()))
            .ReturnsAsync(new GeoResult { Lat = "10.1234", Lon = "20.5678" });

        _weatherClientMock.Setup(w => w.GetWeatherAsync(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<string>()))
            .ReturnsAsync((WeatherSimpleResult?)null);

        var service = CreateService();

        Func<Task> act = async () => await service.GetWeatherAsync("São Paulo", "123");

        await act.Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task Should_Fetch_And_Save_When_Not_In_Cache()
    {
        var location = "São Paulo";
        var apiKey = "123";

        _cacheMock.Setup(c => c.GetCachedResultAsync(location, SearchTypes.Simple))
            .ReturnsAsync((string?)null);

        _geoServiceMock.Setup(g => g.GetGeoDataAsync(location))
            .ReturnsAsync(new GeoResult() { Lat = "10.123456", Lon = "20.654321", Display_Name = "São Paulo" });

        var weather = new WeatherSimpleResult()
        {
            Current = new WeatherSimpleResult.CurrentWeather()
            {
                Temp = 30,
                Feels_Like = 32,
                Wind_Speed = 10,
                Humidity = 50,
                Weather = [new WeatherSimpleResult.WeatherDescription() { Description = "Clear sky" }]
            }
        };

        _weatherClientMock.Setup(w => w.GetWeatherAsync(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<string>()))
            .ReturnsAsync(weather);

        _searchHistoryMock.Setup(r => r.CreateSearchHistoryAsync(It.IsAny<string>(), It.IsAny<string>(), apiKey))
            .Returns(Task.CompletedTask);

        _cacheMock.Setup(c => c.CreateCachedResultAsync(It.IsAny<string>(), It.IsAny<string>(), SearchTypes.Simple))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var result = await service.GetWeatherAsync(location, apiKey);

        // Assert
        result.Should().NotBeNull();
        result.Temperature.Should().Be(30);
        result.Weather.Should().ContainSingle();
    }
}