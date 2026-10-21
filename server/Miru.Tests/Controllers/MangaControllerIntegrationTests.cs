using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Enums;
using Miru.Models;
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

    private static Genre CreateGenre(string name, string? description = null)
    {
        return new Genre { Id = Guid.NewGuid(), Name = name, Description = description };
    }

    private static Character CreateCharacter(string name, CharacterRole role = CharacterRole.Main)
    {
        return new Character
        {
            Id = Guid.NewGuid(),
            Name = name,
            LastName = name + "Last",
            JapaneseName = name + "Jp",
            JapaneseLastName = name + "LastJp",
            Age = "17",
            Gender = CharacterGender.Female,
            Role = role,
            Description = name + " description.",
            ImageSource = "/images/character/" + name
        };
    }

    private static Manga CreateMangaWithGenres(params Genre[] genres)
    {
        var id = Guid.NewGuid();

        return new Manga
        {
            Id = id,
            Title = "Oyasumi Punpun",
            MangaGenres = [.. genres.Select(g => new MangaGenre { MangaId = id, GenreId = g.Id, Genre = g })]
        };
    }

    private static Manga CreateMangaWithCharacters(params Character[] characters)
    {
        var id = Guid.NewGuid();

        return new Manga
        {
            Id = id,
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            MangaCharacters =
            [
                .. characters.Select(c => new MangaCharacter { MangaId = id, CharacterId = c.Id, Character = c })
            ]
        };
    }

    private static void AssertCharacterMapped(Character expected, CharacterResponse actual)
    {
        Assert.Equal(expected.Id, actual.Id);

        Assert.Equal(expected.Name, actual.Name);

        Assert.Equal(expected.LastName, actual.LastName);

        Assert.Equal(expected.JapaneseName, actual.JapaneseName);

        Assert.Equal(expected.JapaneseLastName, actual.JapaneseLastName);

        Assert.Equal(expected.Age, actual.Age);

        Assert.Equal(expected.Gender, actual.Gender);

        Assert.Equal(expected.Role, actual.Role);

        Assert.Equal(expected.Description, actual.Description);

        Assert.Equal(expected.ImageSource, actual.ImageSource);
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

        Assert.Equal(manga.ImageSource, item.ImageSource);
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasNoGenres_ReturnsEmptyGenres()
    {
        _factory.MangaRepository.GetAllAsync().Returns([CreateMangaWithGenres()]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        Assert.Empty(Assert.Single(body!).Genres);
    }

    [Fact]
    public async Task GetAllAsync_MapsGenresCorrectly()
    {
        var drama = CreateGenre("Drama", "Emotionally driven stories.");

        var sliceOfLife = CreateGenre("Slice of Life", "Everyday moments.");

        _factory.MangaRepository.GetAllAsync().Returns([CreateMangaWithGenres(drama, sliceOfLife)]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        var genres = Assert.Single(body!).Genres;

        Assert.Equal(2, genres.Count);

        Assert.Equal(drama.Id, genres[0].Id);

        Assert.Equal(drama.Name, genres[0].Name);

        Assert.Equal(drama.Description, genres[0].Description);

        Assert.Equal(sliceOfLife.Id, genres[1].Id);

        Assert.Equal(sliceOfLife.Name, genres[1].Name);

        Assert.Equal(sliceOfLife.Description, genres[1].Description);
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        _factory.MangaRepository.GetAllAsync().Returns([CreateMangaWithCharacters()]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        Assert.Empty(Assert.Single(body!).Characters);
    }

    [Fact]
    public async Task GetAllAsync_MapsCharactersCorrectly()
    {
        var kaguya = CreateCharacter("Kaguya");

        var miyuki = CreateCharacter("Miyuki", CharacterRole.Supporting);

        _factory.MangaRepository.GetAllAsync().Returns([CreateMangaWithCharacters(kaguya, miyuki)]);

        var body = await _client.GetFromJsonAsync<List<MangaResponse>>("/mangas", JsonOptions);

        var characters = Assert.Single(body!).Characters;

        Assert.Equal(2, characters.Count);

        AssertCharacterMapped(kaguya, characters[0]);

        AssertCharacterMapped(miyuki, characters[1]);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNotFound()
    {
        _factory.MangaRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Manga?)null);

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
        _factory.MangaRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Manga?)null);

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

        Assert.Equal(manga.ImageSource, body.ImageSource);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoGenres_ReturnsEmptyGenres()
    {
        var manga = CreateMangaWithGenres();

        _factory.MangaRepository.GetByIdAsync(manga.Id).Returns(manga);

        var body = await _client.GetFromJsonAsync<MangaResponse>("/mangas/" + manga.Id, JsonOptions);

        Assert.NotNull(body);

        Assert.Empty(body.Genres);
    }

    [Fact]
    public async Task GetByIdAsync_MapsGenresCorrectly()
    {
        var action = CreateGenre("Action", "Fast-paced stories.");

        var adventure = CreateGenre("Adventure", "Journeys and exploration.");

        var manga = CreateMangaWithGenres(action, adventure);

        _factory.MangaRepository.GetByIdAsync(manga.Id).Returns(manga);

        var body = await _client.GetFromJsonAsync<MangaResponse>("/mangas/" + manga.Id, JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(2, body.Genres.Count);

        Assert.Equal(action.Id, body.Genres[0].Id);

        Assert.Equal(action.Name, body.Genres[0].Name);

        Assert.Equal(action.Description, body.Genres[0].Description);

        Assert.Equal(adventure.Id, body.Genres[1].Id);

        Assert.Equal(adventure.Name, body.Genres[1].Name);

        Assert.Equal(adventure.Description, body.Genres[1].Description);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        var manga = CreateMangaWithCharacters();

        _factory.MangaRepository.GetByIdAsync(manga.Id).Returns(manga);

        var body = await _client.GetFromJsonAsync<MangaResponse>("/mangas/" + manga.Id, JsonOptions);

        Assert.NotNull(body);

        Assert.Empty(body.Characters);
    }

    [Fact]
    public async Task GetByIdAsync_MapsCharactersCorrectly()
    {
        var musashi = CreateCharacter("Musashi");

        var matahachi = CreateCharacter("Matahachi", CharacterRole.Supporting);

        var manga = CreateMangaWithCharacters(musashi, matahachi);

        _factory.MangaRepository.GetByIdAsync(manga.Id).Returns(manga);

        var body = await _client.GetFromJsonAsync<MangaResponse>("/mangas/" + manga.Id, JsonOptions);

        Assert.NotNull(body);

        Assert.Equal(2, body.Characters.Count);

        AssertCharacterMapped(musashi, body.Characters[0]);

        AssertCharacterMapped(matahachi, body.Characters[1]);
    }
}
