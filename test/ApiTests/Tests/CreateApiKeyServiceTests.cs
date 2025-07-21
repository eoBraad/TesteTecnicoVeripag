using Application.Services;
using Domain.Exceptions;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;

public class CreateApiKeyServiceTests
{
    private readonly Mock<IKeyRepository> _keyRepositoryMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();

    private CreateApiKeyService CreateService() =>
        new CreateApiKeyService(_keyRepositoryMock.Object, _configurationMock.Object);

    [Fact]
    public async Task Should_Create_ApiKey_When_SecretKey_Is_Valid()
    {
        // Arrange
        var expectedSecretKey = "SuperSecret";
        _configurationMock.Setup(c => c["AppSecretKey"]).Returns(expectedSecretKey);

        _keyRepositoryMock.Setup(r => r.SaveKey(It.IsAny<string>()))
                          .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var apiKey = await service.CreateApiKeyAsync(expectedSecretKey);

        // Assert
        apiKey.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(apiKey, out _).Should().BeTrue(); // Verifica se é um GUID
        _keyRepositoryMock.Verify(r => r.SaveKey(It.Is<string>(k => !string.IsNullOrEmpty(k))), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_SecretKey_Is_Null()
    {
        var service = CreateService();

        Func<Task> act = async () => await service.CreateApiKeyAsync(null);

        await act.Should().ThrowAsync<AppValidationException>();
    }

    [Fact]
    public async Task Should_Throw_When_SecretKey_Is_Empty()
    {
        var service = CreateService();

        Func<Task> act = async () => await service.CreateApiKeyAsync("  ");

        await act.Should().ThrowAsync<AppValidationException>();
    }

    [Fact]
    public async Task Should_Throw_When_SecretKey_Does_Not_Match_AppSecretKey()
    {
        _configurationMock.Setup(c => c["AppSecretKey"]).Returns("CorrectSecret");
        var service = CreateService();

        Func<Task> act = async () => await service.CreateApiKeyAsync("WrongSecret");

        await act.Should().ThrowAsync<AppUnauthorizedException>();

    }
}
