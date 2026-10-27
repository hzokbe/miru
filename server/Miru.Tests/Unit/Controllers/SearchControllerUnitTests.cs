using Microsoft.AspNetCore.Mvc;
using Miru.Controllers;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Controllers;

public class SearchControllerUnitTests
{
    private readonly SearchController _controller;

    private readonly Mock<ISearchService> _service = new();

    public SearchControllerUnitTests()
    {
        _controller = new SearchController(_service.Object);
    }

    [Fact]
    public async Task Search_WhenQueryIsValid_ReturnsOkWithResult()
    {
        var expected = new SearchResponse(
            [.. AnimeSamples.CreateAll().Select(AnimeResponseFactory.From)],
            [.. MangaSamples.CreateAll().Select(MangaResponseFactory.From)],
            [.. CharacterSamples.CreateAll().Select(CharacterResponseFactory.From)]
        );

        _service.Setup(s => s.SearchAsync("frieren")).ReturnsAsync(expected);

        var result = await _controller.SearchAsync("frieren");

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(expected, ok.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task Search_WhenQueryIsBlank_ReturnsBadRequest(string query)
    {
        var result = await _controller.SearchAsync(query);

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task Search_WhenQueryIsBlank_DoesNotCallService()
    {
        await _controller.SearchAsync(" ");

        _service.Verify(s => s.SearchAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Search_CallsServiceOnceWithGivenQuery()
    {
        _service.Setup(s => s.SearchAsync("kaguya")).ReturnsAsync(new SearchResponse([], [], []));

        await _controller.SearchAsync("kaguya");

        _service.Verify(s => s.SearchAsync("kaguya"), Times.Once);
    }
}
