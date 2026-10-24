using Microsoft.AspNetCore.Mvc;
using Miru.Controllers;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Controllers;

public class AnimeControllerUnitTests
{
    private readonly AnimeController _controller;

    private readonly Mock<IAnimeService> _service = new();

    public AnimeControllerUnitTests()
    {
        _controller = new AnimeController(_service.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenServiceReturnsAnimes_ReturnsOkWithAnimes()
    {
        var animes = AnimeSamples.CreateAll().Select(AnimeResponseFactory.From).ToList();

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
    public async Task GetByIdAsync_WhenServiceReturnsAnime_ReturnsOkWithAnime()
    {
        var anime = AnimeResponseFactory.From(AnimeSamples.CreateAll()[0]);

        _service.Setup(s => s.GetByIdAsync(anime.Id)).ReturnsAsync(anime);

        var result = await _controller.GetByIdAsync(anime.Id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(anime, ok.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsNull_ReturnsNotFound()
    {
        _service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((AnimeResponse?)null);

        var result = await _controller.GetByIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundResult>(result);

        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_CallsServiceOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((AnimeResponse?)null);

        await _controller.GetByIdAsync(id);

        _service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }
}
