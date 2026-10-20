using Miru.Enums;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;
using Moq;

namespace Miru.Tests.Services;

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
        var id = Guid.NewGuid();

        var manga = new Manga
        {
            Id = id,
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            Synopsis =
                "Student council president Miyuki Shirogane and vice president Kaguya Shinomiya have feelings for each other, but neither will confess first, so they wage a battle of wits to make the other do it.",
            Chapters = 281,
            Volumes = 28,
            Status = MangaStatus.Finished,
            PublishedFrom = new DateOnly(2015, 5, 19),
            PublishedTo = new DateOnly(2022, 11, 2),
            Score = 8.88m,
            ImageSource = "/mangas" + id
        };

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = await _service.GetAllAsync();

        var response = Assert.Single(result);

        Assert.Equal(manga.Id, response.Id);

        Assert.Equal(manga.Title, response.Title);

        Assert.Equal(manga.JapaneseTitle, response.JapaneseTitle);

        Assert.Equal(manga.Synopsis, response.Synopsis);

        Assert.Equal(manga.Chapters, response.Chapters);

        Assert.Equal(manga.Volumes, response.Volumes);

        Assert.Equal(manga.Status, response.Status);

        Assert.Equal(manga.PublishedFrom, response.PublishedFrom);

        Assert.Equal(manga.PublishedTo, response.PublishedTo);

        Assert.Equal(manga.Score, response.Score);

        Assert.Equal(manga.ImageSource, response.ImageSource);
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
    public async Task GetByIdAsync_WhenRepositoryReturnsManga_ReturnsManga()
    {
        var id = Guid.NewGuid();

        var manga = new Manga { Id = id };

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(manga.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsCorrectly()
    {
        var id = Guid.NewGuid();

        var manga = new Manga
        {
            Id = Guid.NewGuid(),
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            Synopsis =
                "At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.",
            Chapters = 281,
            Volumes = 28,
            Status = MangaStatus.Finished,
            PublishedFrom = new DateOnly(2015, 5, 19),
            PublishedTo = new DateOnly(2022, 11, 2),
            Score = 8.88m
        };

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(manga.Id, result.Id);

        Assert.Equal(manga.Title, result.Title);

        Assert.Equal(manga.JapaneseTitle, result.JapaneseTitle);

        Assert.Equal(manga.Synopsis, result.Synopsis);

        Assert.Equal(manga.Chapters, result.Chapters);

        Assert.Equal(manga.Volumes, result.Volumes);

        Assert.Equal(manga.Status, result.Status);

        Assert.Equal(manga.PublishedFrom, result.PublishedFrom);

        Assert.Equal(manga.PublishedTo, result.PublishedTo);

        Assert.Equal(manga.Score, result.Score);

        Assert.Equal(manga.ImageSource, result.ImageSource);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Manga?)null);

        await _service.GetByIdAsync(Guid.NewGuid());

        _repository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
