using Miru.Data.Seeding;
using Miru.Repositories;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Services;

public class AnimeServiceUnitTests
{
    private readonly Mock<IAnimeRepository> _repository = new();

    private readonly AnimeService _service;

    public AnimeServiceUnitTests()
    {
        _service = new AnimeService(_repository.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsNoAnimes_ReturnsEmptyList()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsCorrectly()
    {
        var anime = AnimeSamples.CreateAll()[0];

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([anime]);

        var result = await _service.GetAllAsync();

        AnimeAssert.Matches(anime, Assert.Single(result));
    }

    [Fact]
    public async Task GetAllAsync_PreservesRepositoryOrder()
    {
        var animes = AnimeSamples.CreateAll();

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(animes);

        var result = await _service.GetAllAsync();

        Assert.Equal(animes.Select(a => a.Id), result.Select(a => a.Id));
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        await _service.GetAllAsync();

        _repository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
