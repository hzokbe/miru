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
                "Student council president Miyuki Shirogane and vice president Kaguya Shinomiya have feelings for each other, but neither will confess first, so they wage a battle of wits to make the other do it.",
                281,
                28,
                MangaStatus.Finished,
                new DateOnly(2015, 5, 19),
                new DateOnly(2022, 11, 2),
                8.88m,
                "/mangas" + id
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
}
