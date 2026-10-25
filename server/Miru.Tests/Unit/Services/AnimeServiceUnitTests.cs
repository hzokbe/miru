using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Models;
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

    private static List<CharacterResponse> ExpectedResponses(IEnumerable<Character> characters)
    {
        return [.. characters.Select(CharacterResponseFactory.ToResponse)];
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

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Anime?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsCorrectly()
    {
        var anime = AnimeSamples.CreateAll()[0];

        _repository.Setup(r => r.GetByIdAsync(anime.Id)).ReturnsAsync(anime);

        var result = await _service.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        AnimeAssert.Matches(anime, result);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Anime?)null);

        await _service.GetByIdAsync(id);

        _repository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_MapsCharacters()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var anime = AnimeCharacterLinker.WithCharacters(AnimeSamples.CreateAll()[0], characters);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([anime]);

        var result = await _service.GetAllAsync();

        Assert.Equal(ExpectedResponses(characters), Assert.Single(result).Characters);
    }

    [Fact]
    public async Task GetAllAsync_WhenAnimeHasNoCharacters_ReturnsEmptyCharacters()
    {
        var anime = AnimeSamples.CreateAll()[0];

        anime.AnimeCharacters = [];

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([anime]);

        var result = await _service.GetAllAsync();

        Assert.Empty(Assert.Single(result).Characters);
    }

    [Fact]
    public async Task GetByIdAsync_MapsCharacters()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var anime = AnimeCharacterLinker.WithCharacters(AnimeSamples.CreateAll()[0], characters);

        _repository.Setup(r => r.GetByIdAsync(anime.Id)).ReturnsAsync(anime);

        var result = await _service.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        Assert.Equal(ExpectedResponses(characters), result.Characters);
    }
}
