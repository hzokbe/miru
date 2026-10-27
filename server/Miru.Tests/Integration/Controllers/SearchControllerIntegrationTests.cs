using System.Net;
using System.Text.Json;
using Miru.Data.Seeding;
using Miru.Tests.Support;
using NSubstitute;

namespace Miru.Tests.Integration.Controllers;

public class SearchControllerIntegrationTests : IClassFixture<MiruFactory>
{
    private readonly HttpClient _client;

    private readonly MiruFactory _factory;

    public SearchControllerIntegrationTests(MiruFactory factory)
    {
        _factory = factory;

        _factory.AnimeRepository.ClearReceivedCalls();

        _factory.MangaRepository.ClearReceivedCalls();

        _factory.CharacterRepository.ClearReceivedCalls();

        _client = factory.CreateSecureClient();
    }

    private void SetupEmpty()
    {
        _factory.AnimeRepository.SearchAsync(Arg.Any<string>()).Returns([]);

        _factory.MangaRepository.SearchAsync(Arg.Any<string>()).Returns([]);

        _factory.CharacterRepository.SearchAsync(Arg.Any<string>()).Returns([]);
    }

    [Fact]
    public async Task Search_WhenRepositoriesReturnResults_ReturnsOkWithAllGroups()
    {
        var animes = AnimeSamples.CreateAll().Take(2).ToList();

        var mangas = MangaSamples.CreateAll().Take(3).ToList();

        var characters = CharacterSamples.CreateAll().Take(4).ToList();

        _factory.AnimeRepository.SearchAsync("frieren").Returns(animes);

        _factory.MangaRepository.SearchAsync("frieren").Returns(mangas);

        _factory.CharacterRepository.SearchAsync("frieren").Returns(characters);

        var response = await _client.GetAsync("/search/frieren");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var root = document.RootElement;

        Assert.Equal(animes.Select(a => a.Id),
            root.GetProperty("animes").EnumerateArray().Select(e => e.GetProperty("id").GetGuid()));

        Assert.Equal(mangas.Select(m => m.Id),
            root.GetProperty("mangas").EnumerateArray().Select(e => e.GetProperty("id").GetGuid()));

        Assert.Equal(characters.Select(c => c.Id),
            root.GetProperty("characters").EnumerateArray().Select(e => e.GetProperty("id").GetGuid()));
    }

    [Fact]
    public async Task Search_WhenNothingMatches_ReturnsOkWithEmptyGroups()
    {
        SetupEmpty();

        var response = await _client.GetAsync("/search/zzzzzz");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var root = document.RootElement;

        Assert.Equal(0, root.GetProperty("animes").GetArrayLength());

        Assert.Equal(0, root.GetProperty("mangas").GetArrayLength());

        Assert.Equal(0, root.GetProperty("characters").GetArrayLength());
    }

    [Fact]
    public async Task Search_ReturnsJsonContentType()
    {
        SetupEmpty();

        var response = await _client.GetAsync("/search/frieren");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Search_WhenQueryIsBlank_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/search/%20");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_WhenQueryIsBlank_DoesNotCallRepositories()
    {
        await _client.GetAsync("/search/%20");

        await _factory.AnimeRepository.DidNotReceive().SearchAsync(Arg.Any<string>());

        await _factory.MangaRepository.DidNotReceive().SearchAsync(Arg.Any<string>());

        await _factory.CharacterRepository.DidNotReceive().SearchAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task Search_CallsEachRepositoryOnceWithGivenQuery()
    {
        SetupEmpty();

        await _client.GetAsync("/search/kaguya");

        await _factory.AnimeRepository.Received(1).SearchAsync("kaguya");

        await _factory.MangaRepository.Received(1).SearchAsync("kaguya");

        await _factory.CharacterRepository.Received(1).SearchAsync("kaguya");
    }
}
