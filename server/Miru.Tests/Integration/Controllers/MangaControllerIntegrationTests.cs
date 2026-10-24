using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Tests.Support;
using NSubstitute;

namespace Miru.Tests.Integration.Controllers;

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
    public async Task GetAllAsync_WhenRepositoryReturnsMangas_ReturnsOkWithMangasInOrder()
    {
        var mangas = MangaSamples.CreateAll();

        _factory.MangaRepository.GetAllAsync().Returns(mangas);

        var response = await _client.GetAsync("/mangas");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<MangaResponse>>(JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(mangas.Select(m => m.Id), body.Select(m => m.Id));

        Assert.Equal(mangas.Select(m => m.Title), body.Select(m => m.Title));
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ReturnsOkWithEmptyList()
    {
        _factory.MangaRepository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/mangas");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<MangaResponse>>(JsonOptions);

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
        var manga = MangaSamples.CreateAll()[0];

        _factory.MangaRepository.GetAllAsync().Returns([manga]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        Assert.NotNull(body);

        MangaAssert.Matches(manga, Assert.Single(body));
    }

    [Fact]
    public async Task GetAllAsync_MapsNullableFieldsFromOngoingManga()
    {
        var manga = MangaSamples.CreateAll().First(m => m.Chapters is null);

        _factory.MangaRepository.GetAllAsync().Returns([manga]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        Assert.NotNull(body);

        MangaAssert.Matches(manga, Assert.Single(body));
    }
}
