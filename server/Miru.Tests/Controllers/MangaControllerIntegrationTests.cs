using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Tests.Fixtures;
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
}
