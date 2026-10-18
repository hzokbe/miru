using Miru.Enums;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;
using Moq;

namespace Miru.Tests.Services;

public class AnimeServiceUnitTests
{
    private readonly Mock<IAnimeRepository> _repository = new();

    private readonly AnimeService _service;

    public AnimeServiceUnitTests()
    {
        _service = new AnimeService(_repository.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsNoAnimes_ReturnsEmptyList()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsCorrectly()
    {
        var id = Guid.NewGuid();

        var anime = new Anime
        {
            Id = id,
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            Synopsis = "Two student council geniuses wage a battle of wits to make the other confess first.",
            Type = AnimeType.Tv,
            Episodes = 12,
            Status = AnimeStatus.FinishedAiring,
            AiredFrom = new DateOnly(2019, 1, 12),
            AiredTo = new DateOnly(2019, 3, 30),
            Score = 8.40m,
            ImageSource = "/images/anime/" + id
        };

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([anime]);

        var result = await _service.GetAllAsync();

        var response = Assert.Single(result);

        Assert.Equal(anime.Id, response.Id);

        Assert.Equal(anime.Title, response.Title);

        Assert.Equal(anime.JapaneseTitle, response.JapaneseTitle);

        Assert.Equal(anime.Synopsis, response.Synopsis);

        Assert.Equal(anime.Type, response.Type);

        Assert.Equal(anime.Episodes, response.Episodes);

        Assert.Equal(anime.Status, response.Status);

        Assert.Equal(anime.AiredFrom, response.AiredFrom);

        Assert.Equal(anime.AiredTo, response.AiredTo);

        Assert.Equal(anime.Score, response.Score);

        Assert.Equal(anime.ImageSource, response.ImageSource);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        await _service.GetAllAsync();

        _repository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
