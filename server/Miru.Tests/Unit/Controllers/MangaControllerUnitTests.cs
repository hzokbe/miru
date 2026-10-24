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
}
