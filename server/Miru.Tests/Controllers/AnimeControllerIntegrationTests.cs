using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Data;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Repositories;
using NSubstitute;

namespace Miru.Tests.Controllers;

public class MiruFactory : WebApplicationFactory<Program>
{
    public IAnimeRepository Repository { get; } = Substitute.For<IAnimeRepository>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();

            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("test"));

            services.RemoveAll<IAnimeRepository>();

            services.AddSingleton(Repository);
        });
    }

    public HttpClient CreateSecureClient()
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }
}

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
    public async Task GetAllAsync_WhenRepositoryReturnsAnimes_ReturnsOkWithAnimes()
    {
        var animes = AnimeSeedData.Create();

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

        var body = await response.Content.ReadFromJsonAsync<List<AnimeResponse>>();

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
        var anime = AnimeSeedData.Create()[0];

        _factory.Repository.GetAllAsync().Returns([anime]);

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
}
