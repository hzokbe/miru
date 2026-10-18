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
                "/images/anime/" + id
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
}
