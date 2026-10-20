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

public class MangaControllerIntegrationTests : IClassFixture<MiruFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client;

    private readonly MiruFactory _factory;

    public MangaControllerIntegrationTests(MiruFactory factory)
    {
        _factory = factory;

        _factory.MangaRepository.ClearReceivedCalls();

        _client = factory.CreateSecureClient();
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsMangas_ReturnsOkWithMangas()
    {
        var mangas = MangaSeedData.Create();

        _factory.MangaRepository.GetAllAsync().Returns(mangas);

        var response = await _client.GetAsync("/mangas");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<MangaResponse>>(JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(mangas.Select(a => a.Id), body.Select(a => a.Id));

        Assert.Equal(mangas.Select(a => a.Title), body.Select(a => a.Title));
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ReturnsOkWithEmptyList()
    {
        _factory.MangaRepository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/mangas");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<MangaResponse>>();

        Assert.NotNull(body);

        Assert.Empty(body);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsJsonContentType()
    {
        _factory.MangaRepository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/mangas");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _factory.MangaRepository.GetAllAsync().Returns([]);

        await _client.GetAsync("/mangas");

        await _factory.MangaRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsFromManga()
    {
        var manga = MangaSeedData.Create()[0];

        _factory.MangaRepository.GetAllAsync().Returns([manga]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        var item = Assert.Single(body!);

        Assert.Equal(manga.Id, item.Id);

        Assert.Equal(manga.Title, item.Title);

        Assert.Equal(manga.JapaneseTitle, item.JapaneseTitle);

        Assert.Equal(manga.Synopsis, item.Synopsis);

        Assert.Equal(manga.Chapters, item.Chapters);

        Assert.Equal(manga.Volumes, item.Volumes);

        Assert.Equal(manga.Status, item.Status);

        Assert.Equal(manga.PublishedFrom, item.PublishedFrom);

        Assert.Equal(manga.PublishedTo, item.PublishedTo);

        Assert.Equal(manga.Score, item.Score);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNotFound()
    {
        _factory.MangaRepository.GetByIdAsync(It.IsAny<Guid>()).Returns((Manga?)null);

        var response = await _client.GetAsync("/mangas/" + Guid.NewGuid());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsManga_ReturnsOkWithManga()
    {
        var manga = MangaSeedData.Create()[0];

        _factory.MangaRepository.GetByIdAsync(manga.Id).Returns(manga);

        var response = await _client.GetAsync("/mangas/" + manga.Id);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<MangaResponse>(JsonOptions);

        Assert.NotNull(body);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProblemPlusJsonContentType()
    {
        _factory.MangaRepository.GetByIdAsync(It.IsAny<Guid>()).Returns((Manga?)null);

        var response = await _client.GetAsync("/mangas/" + Guid.NewGuid());

        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnce()
    {
        var id = Guid.NewGuid();

        _factory.MangaRepository.GetByIdAsync(id).Returns((Manga?)null);

        await _client.GetAsync("/mangas/" + id);

        await _factory.MangaRepository.Received(1).GetByIdAsync(id);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsFromManga()
    {
        var manga = MangaSeedData.Create()[0];

        _factory.MangaRepository.GetByIdAsync(manga.Id).Returns(manga);

        var body = await _client.GetFromJsonAsync<MangaResponse>("/mangas/" + manga.Id, JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(manga.Id, body.Id);

        Assert.Equal(manga.Title, body.Title);

        Assert.Equal(manga.JapaneseTitle, body.JapaneseTitle);

        Assert.Equal(manga.Synopsis, body.Synopsis);

        Assert.Equal(manga.Chapters, body.Chapters);

        Assert.Equal(manga.Volumes, body.Volumes);

        Assert.Equal(manga.Status, body.Status);

        Assert.Equal(manga.PublishedFrom, body.PublishedFrom);

        Assert.Equal(manga.PublishedTo, body.PublishedTo);

        Assert.Equal(manga.Score, body.Score);
    }
}
