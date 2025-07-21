using Application.Services;
using Domain.Entity;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

public class GetSearchHistoryServiceTests
{
    private readonly Mock<ISearchHistoryRepository> _repositoryMock = new();

    private GetSearchHistoryService CreateService() =>
        new GetSearchHistoryService(_repositoryMock.Object);

    [Fact]
    public async Task Should_Return_Search_History_When_Exists()
    {
        // Arrange
        var apiKey = "valid-api-key";
        var expectedHistory = new List<SearchHistory>
        {
            new SearchHistory { Id = Guid.NewGuid(), Result = "São Paulo", CreatedAt = DateTime.UtcNow, ApiKey = "valid-api-key", Query = "São Paulo" },
            new SearchHistory { Id = Guid.NewGuid(), Result = "São Paulo", CreatedAt = DateTime.UtcNow, ApiKey = "valid-api-key", Query = "São Paulo" },
        };

        _repositoryMock.Setup(r => r.GetAllHistoryAsync(apiKey))
            .ReturnsAsync(expectedHistory);

        var service = CreateService();

        // Act
        var result = await service.HandleAsync(apiKey);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(x => x.Query == "São Paulo");
    }

    [Fact]
    public async Task Should_Return_Empty_List_When_No_History_Found()
    {
        // Arrange
        var apiKey = "valid-api-key";
        _repositoryMock.Setup(r => r.GetAllHistoryAsync(apiKey))
            .ReturnsAsync(new List<SearchHistory>());

        var service = CreateService();

        // Act
        var result = await service.HandleAsync(apiKey);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}