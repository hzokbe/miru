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
}
