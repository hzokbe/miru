using Miru.Data.Seeding;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Services;

public class CharacterServiceUnitTests
{
    private readonly Mock<ICharacterRepository> _repository = new();

    private readonly CharacterService _service;

    public CharacterServiceUnitTests()
    {
        _service = new CharacterService(_repository.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Character?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsCorrectly()
    {
        var character = CharacterSamples.CreateAll()[0];

        _repository.Setup(r => r.GetByIdAsync(character.Id)).ReturnsAsync(character);

        var result = await _service.GetByIdAsync(character.Id);

        Assert.NotNull(result);

        Assert.Equal(character.Id, result.Id);

        Assert.Equal(character.Name, result.Name);

        Assert.Equal(character.LastName, result.LastName);

        Assert.Equal(character.JapaneseName, result.JapaneseName);

        Assert.Equal(character.JapaneseLastName, result.JapaneseLastName);

        Assert.Equal(character.Age, result.Age);

        Assert.Equal(character.Gender, result.Gender);

        Assert.Equal(character.Role, result.Role);

        Assert.Equal(character.Description, result.Description);

        Assert.Equal(character.ImageSource, result.ImageSource);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSameResponseAsFactory()
    {
        var character = CharacterSamples.CreateAll()[0];

        _repository.Setup(r => r.GetByIdAsync(character.Id)).ReturnsAsync(character);

        var result = await _service.GetByIdAsync(character.Id);

        Assert.Equal(CharacterResponseFactory.ToResponse(character), result);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Character?)null);

        await _service.GetByIdAsync(id);

        _repository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
