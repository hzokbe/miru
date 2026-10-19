using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Models;
using Miru.Tests.Fixtures;
using Moq;
using NSubstitute;

namespace Miru.Tests.Controllers;

public class AnimeControllerIntegrationTests : IClassFixture<MiruFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client;

    private readonly MiruFactory _factory;

    public AnimeControllerIntegrationTests(MiruFactory factory)
    {
        _factory = factory;

        _factory.AnimeRepository.ClearReceivedCalls();

        _client = factory.CreateSecureClient();
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsAnimes_ReturnsOkWithAnimes()
    {
        var animes = AnimeSeedData.Create();

        _factory.AnimeRepository.GetAllAsync().Returns(animes);

        var response = await _client.GetAsync("/animes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<AnimeResponse>>(JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(animes.Select(a => a.Id), body.Select(a => a.Id));

        Assert.Equal(animes.Select(a => a.Title), body.Select(a => a.Title));
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ReturnsOkWithEmptyList()
    {
        _factory.AnimeRepository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/animes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<AnimeResponse>>();

        Assert.NotNull(body);

        Assert.Empty(body);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsJsonContentType()
    {
        _factory.AnimeRepository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/animes");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _factory.AnimeRepository.GetAllAsync().Returns([]);

        await _client.GetAsync("/animes");

        await _factory.AnimeRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsFromAnime()
    {
        var anime = AnimeSeedData.Create()[0];

        _factory.AnimeRepository.GetAllAsync().Returns([anime]);

        var body = await _client.GetFromJsonAsync<List<AnimeResponse>>("/animes", JsonOptions);

        var item = Assert.Single(body!);

        Assert.Equal(anime.Id, item.Id);

        Assert.Equal(anime.Title, item.Title);

        Assert.Equal(anime.JapaneseTitle, item.JapaneseTitle);

        Assert.Equal(anime.Synopsis, item.Synopsis);

        Assert.Equal(anime.Type, item.Type);

        Assert.Equal(anime.Episodes, item.Episodes);

        Assert.Equal(anime.Status, item.Status);

        Assert.Equal(anime.AiredFrom, item.AiredFrom);

        Assert.Equal(anime.AiredTo, item.AiredTo);

        Assert.Equal(anime.Score, item.Score);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNotFound()
    {
        _factory.AnimeRepository.GetByIdAsync(It.IsAny<Guid>()).Returns((Anime?)null);

        var response = await _client.GetAsync("/animes/" + Guid.NewGuid());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsAnime_ReturnsOkWithAnime()
    {
        var anime = AnimeSeedData.Create()[0];

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var response = await _client.GetAsync("/animes/" + anime.Id);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<AnimeResponse>(JsonOptions);

        Assert.NotNull(body);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProblemPlusJsonContentType()
    {
        _factory.AnimeRepository.GetByIdAsync(It.IsAny<Guid>()).Returns((Anime?)null);

        var response = await _client.GetAsync("/animes/" + Guid.NewGuid());

        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnce()
    {
        var id = Guid.NewGuid();

        _factory.AnimeRepository.GetByIdAsync(id).Returns((Anime?)null);

        await _client.GetAsync("/animes/" + id);

        await _factory.AnimeRepository.Received(1).GetByIdAsync(id);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsFromAnime()
    {
        var anime = AnimeSeedData.Create()[0];

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var body = await _client.GetFromJsonAsync<AnimeResponse>("/animes/" + anime.Id, JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(anime.Id, body.Id);

        Assert.Equal(anime.Title, body.Title);

        Assert.Equal(anime.JapaneseTitle, body.JapaneseTitle);

        Assert.Equal(anime.Synopsis, body.Synopsis);

        Assert.Equal(anime.Type, body.Type);

        Assert.Equal(anime.Episodes, body.Episodes);

        Assert.Equal(anime.Status, body.Status);

        Assert.Equal(anime.AiredFrom, body.AiredFrom);

        Assert.Equal(anime.AiredTo, body.AiredTo);

        Assert.Equal(anime.Score, body.Score);
    }
}
