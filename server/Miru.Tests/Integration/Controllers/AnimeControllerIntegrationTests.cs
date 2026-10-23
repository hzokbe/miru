using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Tests.Support;
using NSubstitute;

namespace Miru.Tests.Integration.Controllers;

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

        _factory.Repository.ClearReceivedCalls();

        _client = factory.CreateSecureClient();
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsAnimes_ReturnsOkWithAnimesInOrder()
    {
        var animes = AnimeSamples.CreateAll();

        _factory.Repository.GetAllAsync().Returns(animes);

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
        _factory.Repository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/animes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<AnimeResponse>>(JsonOptions);

        Assert.NotNull(body);

        Assert.Empty(body);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsJsonContentType()
    {
        _factory.Repository.GetAllAsync().Returns([]);

        var response = await _client.GetAsync("/animes");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _factory.Repository.GetAllAsync().Returns([]);

        await _client.GetAsync("/animes");

        await _factory.Repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsFromAnime()
    {
        var anime = AnimeSamples.CreateAll()[0];

        _factory.Repository.GetAllAsync().Returns([anime]);

        var body = await _client.GetFromJsonAsync<List<AnimeResponse>>("/animes", JsonOptions);

        Assert.NotNull(body);

        AnimeAssert.Matches(anime, Assert.Single(body));
    }
}
