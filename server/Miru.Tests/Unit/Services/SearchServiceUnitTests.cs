using Miru.Data.Seeding;
using Miru.Repositories;
using Miru.Services;
using Moq;

namespace Miru.Tests.Unit.Services;

public class SearchServiceUnitTests
{
    private readonly Mock<IAnimeRepository> _animeRepository = new();

    private readonly Mock<ICharacterRepository> _characterRepository = new();

    private readonly Mock<IMangaRepository> _mangaRepository = new();

    private readonly SearchService _service;

    public SearchServiceUnitTests()
    {
        _service = new SearchService(_animeRepository.Object, _mangaRepository.Object, _characterRepository.Object);
    }

    [Fact]
    public async Task SearchAsync_WhenNothingMatches_ReturnsEmptyLists()
    {
        _animeRepository.Setup(r => r.SearchAsync(It.IsAny<string>())).ReturnsAsync([]);

        _mangaRepository.Setup(r => r.SearchAsync(It.IsAny<string>())).ReturnsAsync([]);

        _characterRepository.Setup(r => r.SearchAsync(It.IsAny<string>())).ReturnsAsync([]);

        var result = await _service.SearchAsync("nothing");

        Assert.Empty(result.Animes);

        Assert.Empty(result.Mangas);

        Assert.Empty(result.Characters);
    }

    [Fact]
    public async Task SearchAsync_CombinesResultsFromAllRepositories()
    {
        var animes = AnimeSamples.CreateAll().Take(2).ToList();

        var mangas = MangaSamples.CreateAll().Take(3).ToList();

        var characters = CharacterSamples.CreateAll().Take(4).ToList();

        _animeRepository.Setup(r => r.SearchAsync("query")).ReturnsAsync(animes);

        _mangaRepository.Setup(r => r.SearchAsync("query")).ReturnsAsync(mangas);

        _characterRepository.Setup(r => r.SearchAsync("query")).ReturnsAsync(characters);

        var result = await _service.SearchAsync("query");

        Assert.Equal(animes.Select(a => a.Id), result.Animes.Select(a => a.Id));

        Assert.Equal(mangas.Select(m => m.Id), result.Mangas.Select(m => m.Id));

        Assert.Equal(characters.Select(c => c.Id), result.Characters.Select(c => c.Id));
    }

    [Fact]
    public async Task SearchAsync_CallsEachRepositoryOnceWithGivenQuery()
    {
        _animeRepository.Setup(r => r.SearchAsync("query")).ReturnsAsync([]);

        _mangaRepository.Setup(r => r.SearchAsync("query")).ReturnsAsync([]);

        _characterRepository.Setup(r => r.SearchAsync("query")).ReturnsAsync([]);

        await _service.SearchAsync("query");

        _animeRepository.Verify(r => r.SearchAsync("query"), Times.Once);

        _mangaRepository.Verify(r => r.SearchAsync("query"), Times.Once);

        _characterRepository.Verify(r => r.SearchAsync("query"), Times.Once);
    }
}
