using Microsoft.AspNetCore.Mvc;
using Miru.Controllers;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Controllers;

public class MangaControllerUnitTests
{
    private readonly MangaController _controller;

    private readonly Mock<IMangaService> _service = new();

    public MangaControllerUnitTests()
    {
        _controller = new MangaController(_service.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenServiceReturnsMangas_ReturnsOkWithMangas()
    {
        var mangas = MangaSamples.CreateAll().Select(MangaResponseFactory.From).ToList();

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
    public async Task GetByIdAsync_WhenServiceReturnsManga_ReturnsOkWithManga()
    {
        var manga = MangaResponseFactory.From(MangaSamples.CreateAll()[0]);

        _service.Setup(s => s.GetByIdAsync(manga.Id)).ReturnsAsync(manga);

        var result = await _controller.GetByIdAsync(manga.Id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(manga, ok.Value);
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
    public async Task GetByIdAsync_CallsServiceOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((MangaResponse?)null);

        await _controller.GetByIdAsync(id);

        _service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }
}
