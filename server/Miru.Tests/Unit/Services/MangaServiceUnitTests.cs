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
}
