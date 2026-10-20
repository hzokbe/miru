using Microsoft.AspNetCore.Mvc;
using Miru.Controllers;
using Miru.DTO;
using Miru.Enums;
using Miru.Services;
using Moq;

namespace Miru.Tests.Controllers;

public class AnimeControllerUnitTests
{
    private readonly AnimeController _controller;

    private readonly Mock<IAnimeService> _service = new();

    public AnimeControllerUnitTests()
    {
        _controller = new AnimeController(_service.Object);
    }

    private static List<GenreResponse> CreateGenres()
    {
        return
        [
            new GenreResponse(Guid.NewGuid(), "Comedy", "Humor-focused stories."),
            new GenreResponse(Guid.NewGuid(), "Romance", "Stories centered on love.")
        ];
    }

    [Fact]
    public async Task GetAllAsync_WhenServiceReturnsAnimes_ReturnsOkWithAnimes()
    {
        var id = Guid.NewGuid();

        var animes = new List<AnimeResponse>
        {
            new(
                id,
                "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
                "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
                "Two student council geniuses wage a battle of wits to make the other confess first.",
                AnimeType.Tv,
                26,
                AnimeStatus.FinishedAiring,
                new DateOnly(2019, 1, 12),
                new DateOnly(2019, 3, 30),
                8.40m,
                "/images/anime/" + id,
                []
            )
        };

        _service.Setup(s => s.GetAllAsync()).ReturnsAsync(animes);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(animes, ok.Value);
    }

    [Fact]
    public async Task GetAllAsync_WhenServiceReturnsNoAnimes_ReturnsOkWithEmptyList()
    {
        _service.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Empty(Assert.IsType<List<AnimeResponse>>(ok.Value));
    }

    [Fact]
    public async Task GetAllAsync_CallsServiceOnce()
    {
        _service.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        await _controller.GetAllAsync();

        _service.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenAnimeHasGenres_ReturnsOkWithGenres()
    {
        var id = Guid.NewGuid();

        var genres = CreateGenres();

        var animes = new List<AnimeResponse>
        {
            new(
                id,
                "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
                null,
                null,
                AnimeType.Tv,
                12,
                AnimeStatus.FinishedAiring,
                null,
                null,
                8.40m,
                null,
                genres
            )
        };

        _service.Setup(s => s.GetAllAsync()).ReturnsAsync(animes);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var body = Assert.IsType<List<AnimeResponse>>(ok.Value);

        Assert.Equal(genres, Assert.Single(body).Genres);
    }

    [Fact]
    public async Task GetAllAsync_WhenAnimeHasNoGenres_ReturnsOkWithEmptyGenres()
    {
        var animes = new List<AnimeResponse>
        {
            new(Guid.NewGuid(), "Steins;Gate", null, null, AnimeType.Tv, 24, AnimeStatus.FinishedAiring, null, null,
                null, null, [])
        };

        _service.Setup(s => s.GetAllAsync()).ReturnsAsync(animes);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var body = Assert.IsType<List<AnimeResponse>>(ok.Value);

        Assert.Empty(Assert.Single(body).Genres);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsNull_ReturnsNotFound()
    {
        _service.Setup(s => s.GetByIdAsync(Guid.NewGuid())).ReturnsAsync((AnimeResponse?)null);

        var result = await _controller.GetByIdAsync(Guid.NewGuid());

        var ok = Assert.IsType<NotFoundResult>(result);

        Assert.Equal(404, ok.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsAnime_ReturnsOkWithAnime()
    {
        var id = Guid.NewGuid();

        var anime = new AnimeResponse(
            id,
            "Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen",
            "かぐや様は告らせたい？ ～天才たちの恋愛頭脳戦～",
            "As the council's term nears its end, a heated election and a birthday complicate the game.",
            AnimeType.Tv,
            12,
            AnimeStatus.FinishedAiring,
            new DateOnly(2020, 4, 11),
            new DateOnly(2020, 6, 27),
            8.61m,
            "/images/anime/" + id,
            []
        );

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(anime);

        var result = await _controller.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(anime, ok.Value);
    }

    [Fact]
    public async Task GetByIdAsync_CallsServiceOnce()
    {
        _service.Setup(s => s.GetByIdAsync(Guid.NewGuid())).ReturnsAsync((AnimeResponse?)null);

        await _controller.GetAllAsync();

        _service.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasGenres_ReturnsOkWithGenres()
    {
        var id = Guid.NewGuid();

        var genres = CreateGenres();

        var anime = new AnimeResponse(
            id,
            "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            null,
            null,
            AnimeType.Tv,
            26,
            AnimeStatus.FinishedAiring,
            null,
            null,
            null,
            null,
            genres
        );

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(anime);

        var result = await _controller.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);

        var body = Assert.IsType<AnimeResponse>(ok.Value);

        Assert.Equal(genres, body.Genres);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasNoGenres_ReturnsOkWithEmptyGenres()
    {
        var id = Guid.NewGuid();

        var anime = new AnimeResponse(
            id,
            "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            null,
            null,
            AnimeType.Tv,
            26,
            AnimeStatus.FinishedAiring,
            null,
            null,
            null,
            null,
            []
        );

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(anime);

        var result = await _controller.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Empty(Assert.IsType<AnimeResponse>(ok.Value).Genres);
    }
}
