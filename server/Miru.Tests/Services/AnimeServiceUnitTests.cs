using Miru.Enums;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;
using Moq;

namespace Miru.Tests.Services;

public class AnimeServiceUnitTests
{
    private readonly Mock<IAnimeRepository> _repository = new();

    private readonly AnimeService _service;

    public AnimeServiceUnitTests()
    {
        _service = new AnimeService(_repository.Object);
    }

    private static Genre CreateGenre(string name, string? description = null)
    {
        return new Genre { Id = Guid.NewGuid(), Name = name, Description = description };
    }

    private static Anime CreateAnimeWithGenres(Guid id, params Genre[] genres)
    {
        return new Anime
        {
            Id = id,
            AnimeGenres = [.. genres.Select(g => new AnimeGenre { AnimeId = id, GenreId = g.Id, Genre = g })]
        };
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
        var id = Guid.NewGuid();

        var anime = new Anime
        {
            Id = id,
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            Synopsis = "Two student council geniuses wage a battle of wits to make the other confess first.",
            Type = AnimeType.Tv,
            Episodes = 12,
            Status = AnimeStatus.FinishedAiring,
            AiredFrom = new DateOnly(2019, 1, 12),
            AiredTo = new DateOnly(2019, 3, 30),
            Score = 8.40m,
            ImageSource = "/images/anime/" + id
        };

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([anime]);

        var result = await _service.GetAllAsync();

        var response = Assert.Single(result);

        Assert.Equal(anime.Id, response.Id);

        Assert.Equal(anime.Title, response.Title);

        Assert.Equal(anime.JapaneseTitle, response.JapaneseTitle);

        Assert.Equal(anime.Synopsis, response.Synopsis);

        Assert.Equal(anime.Type, response.Type);

        Assert.Equal(anime.Episodes, response.Episodes);

        Assert.Equal(anime.Status, response.Status);

        Assert.Equal(anime.AiredFrom, response.AiredFrom);

        Assert.Equal(anime.AiredTo, response.AiredTo);

        Assert.Equal(anime.Score, response.Score);

        Assert.Equal(anime.ImageSource, response.ImageSource);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        await _service.GetAllAsync();

        _repository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenAnimeHasNoGenres_ReturnsEmptyGenres()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([new Anime { Id = Guid.NewGuid() }]);

        var result = await _service.GetAllAsync();

        Assert.Empty(Assert.Single(result).Genres);
    }

    [Fact]
    public async Task GetAllAsync_MapsGenresCorrectly()
    {
        var comedy = CreateGenre("Comedy", "Humor-focused stories.");

        var romance = CreateGenre("Romance", "Stories centered on love.");

        var anime = CreateAnimeWithGenres(Guid.NewGuid(), comedy, romance);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([anime]);

        var result = await _service.GetAllAsync();

        var genres = Assert.Single(result).Genres;

        Assert.Equal(2, genres.Count);

        Assert.Equal(comedy.Id, genres[0].Id);

        Assert.Equal(comedy.Name, genres[0].Name);

        Assert.Equal(comedy.Description, genres[0].Description);

        Assert.Equal(romance.Id, genres[1].Id);

        Assert.Equal(romance.Name, genres[1].Name);

        Assert.Equal(romance.Description, genres[1].Description);
    }

    [Fact]
    public async Task GetAllAsync_MapsGenresForEachAnime()
    {
        var comedy = CreateGenre("Comedy");

        var drama = CreateGenre("Drama");

        var first = CreateAnimeWithGenres(Guid.NewGuid(), comedy);

        var second = CreateAnimeWithGenres(Guid.NewGuid(), drama);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([first, second]);

        var result = await _service.GetAllAsync();

        Assert.Equal(["Comedy"], result[0].Genres.Select(g => g.Name));

        Assert.Equal(["Drama"], result[1].Genres.Select(g => g.Name));
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Anime?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsAnime_ReturnsAnime()
    {
        var id = Guid.NewGuid();

        var anime = new Anime { Id = id };

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(anime);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(anime.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsCorrectly()
    {
        var id = Guid.NewGuid();

        var anime = new Anime
        {
            Id = Guid.NewGuid(),
            Title = "Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい？ ～天才たちの恋愛頭脳戦～",
            Synopsis = "As the council's term nears its end, a heated election and a birthday complicate the game.",
            Type = AnimeType.Tv,
            Episodes = 12,
            Status = AnimeStatus.FinishedAiring,
            AiredFrom = new DateOnly(2020, 4, 11),
            AiredTo = new DateOnly(2020, 6, 27),
            Score = 8.61m
        };

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(anime);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(anime.Id, result.Id);

        Assert.Equal(anime.Title, result.Title);

        Assert.Equal(anime.JapaneseTitle, result.JapaneseTitle);

        Assert.Equal(anime.Synopsis, result.Synopsis);

        Assert.Equal(anime.Type, result.Type);

        Assert.Equal(anime.Episodes, result.Episodes);

        Assert.Equal(anime.Status, result.Status);

        Assert.Equal(anime.AiredFrom, result.AiredFrom);

        Assert.Equal(anime.AiredTo, result.AiredTo);

        Assert.Equal(anime.Score, result.Score);

        Assert.Equal(anime.ImageSource, result.ImageSource);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Anime?)null);

        await _service.GetByIdAsync(Guid.NewGuid());

        _repository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasNoGenres_ReturnsEmptyGenres()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Anime { Id = id });

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Empty(result.Genres);
    }

    [Fact]
    public async Task GetByIdAsync_MapsGenresCorrectly()
    {
        var id = Guid.NewGuid();

        var action = CreateGenre("Action", "Fast-paced stories.");

        var fantasy = CreateGenre("Fantasy", "Worlds with magic.");

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CreateAnimeWithGenres(id, action, fantasy));

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(2, result.Genres.Count);

        Assert.Equal(action.Id, result.Genres[0].Id);

        Assert.Equal(action.Name, result.Genres[0].Name);

        Assert.Equal(action.Description, result.Genres[0].Description);

        Assert.Equal(fantasy.Id, result.Genres[1].Id);

        Assert.Equal(fantasy.Name, result.Genres[1].Name);

        Assert.Equal(fantasy.Description, result.Genres[1].Description);
    }
}
