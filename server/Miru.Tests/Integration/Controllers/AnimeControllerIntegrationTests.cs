using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Models;
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

        _factory.AnimeRepository.ClearReceivedCalls();

        _client = factory.CreateSecureClient();
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsAnimes_ReturnsOkWithAnimesInOrder()
    {
        var animes = AnimeSamples.CreateAll();

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

        var body = await response.Content.ReadFromJsonAsync<List<AnimeResponse>>(JsonOptions);

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
        var anime = AnimeSamples.CreateAll()[0];

        _factory.AnimeRepository.GetAllAsync().Returns([anime]);

        var body = await _client.GetFromJsonAsync<List<AnimeResponse>>("/animes", JsonOptions);

        Assert.NotNull(body);

        AnimeAssert.Matches(anime, Assert.Single(body));
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeExists_ReturnsOkWithAnime()
    {
        var anime = AnimeSamples.CreateAll()[0];

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var response = await _client.GetAsync($"/animes/{anime.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<AnimeResponse>(JsonOptions);

        Assert.NotNull(body);

        AnimeAssert.Matches(anime, body);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeDoesNotExist_ReturnsNotFound()
    {
        _factory.AnimeRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Anime?)null);

        var response = await _client.GetAsync($"/animes/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdIsNotAGuid_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/animes/not-a-guid");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _factory.AnimeRepository.GetByIdAsync(id).Returns((Anime?)null);

        await _client.GetAsync($"/animes/{id}");

        await _factory.AnimeRepository.Received(1).GetByIdAsync(id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCharactersOfAnime()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var anime = AnimeCharacterLinker.WithCharacters(AnimeSamples.CreateAll()[0], characters);

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var body = await _client.GetFromJsonAsync<AnimeResponse>($"/animes/{anime.Id}", JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(characters.Select(c => c.Id), body.Characters.Select(c => c.Id));

        Assert.Equal(characters.Select(c => c.Name), body.Characters.Select(c => c.Name));

        Assert.Equal(characters.Select(c => c.Gender), body.Characters.Select(c => c.Gender));

        Assert.Equal(characters.Select(c => c.Role), body.Characters.Select(c => c.Role));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCharactersForEachAnime()
    {
        var characters = CharacterSamples.CreateAll().Take(2).ToList();

        var anime = AnimeCharacterLinker.WithCharacters(AnimeSamples.CreateAll()[0], characters);

        _factory.AnimeRepository.GetAllAsync().Returns([anime]);

        var body = await _client.GetFromJsonAsync<List<AnimeResponse>>("/animes", JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(characters.Select(c => c.Id), Assert.Single(body).Characters.Select(c => c.Id));
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasNoCharacters_ReturnsEmptyCharacters()
    {
        var anime = AnimeSamples.CreateAll()[0];

        anime.AnimeCharacters = [];

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var body = await _client.GetFromJsonAsync<AnimeResponse>($"/animes/{anime.Id}", JsonOptions);

        Assert.NotNull(body);

        Assert.Empty(body.Characters);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGenresOfAnime()
    {
        var genres = GenreSamples.CreateAll().Take(3).ToList();

        var anime = AnimeGenreLinker.WithGenres(AnimeSamples.CreateAll()[0], genres);

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var body = await _client.GetFromJsonAsync<AnimeResponse>($"/animes/{anime.Id}", JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(genres.Select(g => g.Id), body.Genres.Select(g => g.Id));

        Assert.Equal(genres.Select(g => g.Name), body.Genres.Select(g => g.Name));

        Assert.Equal(genres.Select(g => g.Description), body.Genres.Select(g => g.Description));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsGenresForEachAnime()
    {
        var genres = GenreSamples.CreateAll().Take(2).ToList();

        var anime = AnimeGenreLinker.WithGenres(AnimeSamples.CreateAll()[0], genres);

        _factory.AnimeRepository.GetAllAsync().Returns([anime]);

        var body = await _client.GetFromJsonAsync<List<AnimeResponse>>("/animes", JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(genres.Select(g => g.Id), Assert.Single(body).Genres.Select(g => g.Id));
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasNoGenres_ReturnsEmptyGenres()
    {
        var anime = AnimeSamples.CreateAll()[0];

        anime.AnimeGenres = [];

        _factory.AnimeRepository.GetByIdAsync(anime.Id).Returns(anime);

        var body = await _client.GetFromJsonAsync<AnimeResponse>($"/animes/{anime.Id}", JsonOptions);

        Assert.NotNull(body);

        Assert.Empty(body.Genres);
    }
}
