using Microsoft.AspNetCore.Mvc;
using Miru.Controllers;
using Miru.DTO;
using Miru.Enums;
using Miru.Services;
using Moq;

namespace Miru.Tests.Controllers;

public class MangaControllerUnitTests
{
    private readonly MangaController _controller;

    private readonly Mock<IMangaService> _service = new();

    public MangaControllerUnitTests()
    {
        _controller = new MangaController(_service.Object);
    }

    private static List<GenreResponse> CreateGenres()
    {
        return
        [
            new GenreResponse(
                Guid.NewGuid(),
                "Drama",
                "Emotionally driven stories centered on realistic conflicts and character development."
            ),
            new GenreResponse(
                Guid.NewGuid(),
                "Slice of Life",
                "Low-stakes stories about everyday moments and ordinary people."
            )
        ];
    }

    [Fact]
    public async Task GetAllAsync_WhenServiceReturnsMangas_ReturnsOkWithMangas()
    {
        var id = Guid.NewGuid();

        var mangas = new List<MangaResponse>
        {
            new(
                id,
                "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
                "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
                "At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.",
                281,
                28,
                MangaStatus.Finished,
                new DateOnly(2015, 5, 19),
                new DateOnly(2022, 11, 2),
                8.88m,
                "/images/manga/" + id,
                []
            )
        };

        _service.Setup(s => s.GetAllAsync()).ReturnsAsync(mangas);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(mangas, ok.Value);
    }

    [Fact]
    public async Task GetAllAsync_WhenServiceReturnsNoMangas_ReturnsOkWithEmptyList()
    {
        _service.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Empty(Assert.IsType<List<MangaResponse>>(ok.Value));
    }

    [Fact]
    public async Task GetAllAsync_CallsServiceOnce()
    {
        _service.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        await _controller.GetAllAsync();

        _service.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasGenres_ReturnsOkWithGenres()
    {
        var genres = CreateGenres();

        var mangas = new List<MangaResponse>
        {
            new(
                Guid.NewGuid(),
                "Oyasumi Punpun",
                "おやすみプンプン",
                null,
                147,
                13,
                MangaStatus.Finished,
                new DateOnly(2007, 3, 15),
                new DateOnly(2013, 11, 2),
                8.97m,
                null,
                genres
            )
        };

        _service.Setup(s => s.GetAllAsync()).ReturnsAsync(mangas);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var body = Assert.IsType<List<MangaResponse>>(ok.Value);

        Assert.Equal(genres, Assert.Single(body).Genres);
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasNoGenres_ReturnsOkWithEmptyGenres()
    {
        var mangas = new List<MangaResponse>
        {
            new(
                Guid.NewGuid(),
                "Yotsuba to!",
                "よつばと!",
                null,
                null,
                null,
                MangaStatus.Publishing,
                new DateOnly(2003, 1, 21),
                null,
                8.92m,
                null,
                []
            )
        };

        _service.Setup(s => s.GetAllAsync()).ReturnsAsync(mangas);

        var result = await _controller.GetAllAsync();

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var body = Assert.IsType<List<MangaResponse>>(ok.Value);

        Assert.Empty(Assert.Single(body).Genres);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsNull_ReturnsNotFound()
    {
        _service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((MangaResponse?)null);

        var result = await _controller.GetByIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundResult>(result);

        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsManga_ReturnsOkWithManga()
    {
        var id = Guid.NewGuid();

        var manga = new MangaResponse(
            id,
            "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            "At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.",
            281,
            28,
            MangaStatus.Finished,
            new DateOnly(2015, 5, 19),
            new DateOnly(2022, 11, 2),
            8.88m,
            "/images/manga/" + id,
            []
        );

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _controller.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(manga, ok.Value);
    }

    [Fact]
    public async Task GetByIdAsync_CallsServiceOnce()
    {
        _service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((MangaResponse?)null);

        await _controller.GetByIdAsync(Guid.NewGuid());

        _service.Verify(s => s.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasGenres_ReturnsOkWithGenres()
    {
        var id = Guid.NewGuid();

        var genres = CreateGenres();

        var manga = new MangaResponse(
            id,
            "Oyasumi Punpun",
            "おやすみプンプン",
            null,
            147,
            13,
            MangaStatus.Finished,
            new DateOnly(2007, 3, 15),
            new DateOnly(2013, 11, 2),
            8.97m,
            null,
            genres
        );

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _controller.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);

        var body = Assert.IsType<MangaResponse>(ok.Value);

        Assert.Equal(genres, body.Genres);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoGenres_ReturnsOkWithEmptyGenres()
    {
        var id = Guid.NewGuid();

        var manga = new MangaResponse(
            id,
            "Yotsuba to!",
            "よつばと!",
            null,
            null,
            null,
            MangaStatus.Publishing,
            new DateOnly(2003, 1, 21),
            null,
            8.92m,
            null,
            []
        );

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _controller.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Empty(Assert.IsType<MangaResponse>(ok.Value).Genres);
    }
}
