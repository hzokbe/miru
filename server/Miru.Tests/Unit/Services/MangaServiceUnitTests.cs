using Miru.Data.Seeding;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;
using Miru.Tests.Support;
using Moq;

namespace Miru.Tests.Unit.Services;

public class MangaServiceUnitTests
{
    private readonly Mock<IMangaRepository> _repository = new();

    private readonly MangaService _service;

    public MangaServiceUnitTests()
    {
        _service = new MangaService(_repository.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsNoMangas_ReturnsEmptyList()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsCorrectly()
    {
        var manga = MangaSamples.CreateAll()[0];

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = await _service.GetAllAsync();

        MangaAssert.Matches(manga, Assert.Single(result));
    }

    [Fact]
    public async Task GetAllAsync_MapsNullableFieldsCorrectly()
    {
        var manga = MangaSamples.CreateAll().First(m => m.Chapters is null);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = Assert.Single(await _service.GetAllAsync());

        Assert.Null(result.Chapters);

        Assert.Null(result.Volumes);

        Assert.Null(result.PublishedTo);
    }

    [Fact]
    public async Task GetAllAsync_PreservesRepositoryOrder()
    {
        var mangas = MangaSamples.CreateAll();

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(mangas);

        var result = await _service.GetAllAsync();

        Assert.Equal(mangas.Select(m => m.Id), result.Select(m => m.Id));
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        await _service.GetAllAsync();

        _repository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Manga?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsCorrectly()
    {
        var manga = MangaSamples.CreateAll()[0];

        _repository.Setup(r => r.GetByIdAsync(manga.Id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        MangaAssert.Matches(manga, result);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Manga?)null);

        await _service.GetByIdAsync(id);

        _repository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_MapsCharactersCorrectly()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var manga = MangaCharacterLinker.WithCharacters(MangaSamples.CreateAll()[0], characters);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = Assert.Single(await _service.GetAllAsync());

        Assert.Equal(characters.Select(c => c.Id), result.Characters.Select(c => c.Id));

        Assert.Equal(characters.Select(c => c.Name), result.Characters.Select(c => c.Name));
    }

    [Fact]
    public async Task GetAllAsync_MapsAllCharacterFieldsCorrectly()
    {
        var character = CharacterSamples.CreateAll()[0];

        var manga = MangaCharacterLinker.WithCharacters(MangaSamples.CreateAll()[0], [character]);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = Assert.Single(Assert.Single(await _service.GetAllAsync()).Characters);

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
    public async Task GetAllAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        var manga = MangaSamples.CreateAll()[0];

        manga.MangaCharacters = [];

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = Assert.Single(await _service.GetAllAsync());

        Assert.Empty(result.Characters);
    }

    [Fact]
    public async Task GetByIdAsync_MapsCharactersCorrectly()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var manga = MangaCharacterLinker.WithCharacters(MangaSamples.CreateAll()[0], characters);

        _repository.Setup(r => r.GetByIdAsync(manga.Id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Equal(characters.Select(c => c.Id), result.Characters.Select(c => c.Id));
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        var manga = MangaSamples.CreateAll()[0];

        manga.MangaCharacters = [];

        _repository.Setup(r => r.GetByIdAsync(manga.Id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Empty(result.Characters);
    }
}
