using Microsoft.AspNetCore.Mvc;
using Miru.Controllers;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Controllers;

public class CharacterControllerUnitTests
{
    private readonly CharacterController _controller;

    private readonly Mock<ICharacterService> _service = new();

    public CharacterControllerUnitTests()
    {
        _controller = new CharacterController(_service.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsCharacter_ReturnsOkWithCharacter()
    {
        var character = CharacterResponseFactory.ToResponse(CharacterSamples.CreateAll()[0]);

        _service.Setup(s => s.GetByIdAsync(character.Id)).ReturnsAsync(character);

        var result = await _controller.GetByIdAsync(character.Id);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        Assert.Same(character, ok.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceReturnsNull_ReturnsNotFound()
    {
        _service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CharacterResponse?)null);

        var result = await _controller.GetByIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundResult>(result);

        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_CallsServiceOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _service.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((CharacterResponse?)null);

        await _controller.GetByIdAsync(id);

        _service.Verify(s => s.GetByIdAsync(id), Times.Once);
    }
}
