using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Enums;
using Miru.Models;
using Miru.Repositories;

namespace Miru.Tests.Repositories;

public class AnimeRepositoryUnitTests
{
    private static DbContextOptions<AppDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
    }

    private static async Task SeedAsync(DbContextOptions<AppDbContext> options, params Anime[] animes)
    {
        await using var context = new AppDbContext(options);

        context.Animes.AddRange(animes);

        await context.SaveChangesAsync();
    }

    private static Anime CreateAnime(string title, decimal? score)
    {
        var anime = new Anime
        {
            Id = Guid.NewGuid(),
            Title = title,
            Score = score
        };

        return anime;
    }

    private static Genre CreateGenre(string name)
    {
        return new Genre { Id = Guid.NewGuid(), Name = name };
    }

    private static Character CreateCharacter(string name)
    {
        return new Character
        {
            Id = Guid.NewGuid(),
            Name = name,
            Gender = CharacterGender.Unknown,
            Role = CharacterRole.Main
        };
    }

    private static Anime CreateAnimeWithGenres(string title, decimal? score, params Genre[] genres)
    {
        var anime = CreateAnime(title, score);

        anime.AnimeGenres = [.. genres.Select(g => new AnimeGenre { AnimeId = anime.Id, GenreId = g.Id, Genre = g })];

        return anime;
    }

    private static Anime CreateAnimeWithCharacters(string title, decimal? score, params Character[] characters)
    {
        var anime = CreateAnime(title, score);

        anime.AnimeCharacters =
        [
            .. characters.Select(c => new AnimeCharacter { AnimeId = anime.Id, CharacterId = c.Id, Character = c })
        ];

        return anime;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenThereAreNoAnimes()
    {
        await using var context = new AppDbContext(CreateOptions());

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAnimesOrderedByScore()
    {
        var options = CreateOptions();

        await SeedAsync(
            options,
            CreateAnime("Steins;Gate", 9.07m),
            CreateAnime("Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen", 8.40m),
            CreateAnime("Sousou no Frieren", 9.24m)
        );

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        string[] expected =
        [
            "Sousou no Frieren",
            "Steins;Gate",
            "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen"
        ];

        Assert.Equal(expected, result.Select(a => a.Title));
    }

    [Fact]
    public async Task GetAllAsync_LoadsGenres()
    {
        var options = CreateOptions();

        var comedy = CreateGenre("Comedy");

        var romance = CreateGenre("Romance");

        await SeedAsync(options, CreateAnimeWithGenres("Kaguya-sama", 8.40m, comedy, romance));

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        var anime = Assert.Single(result);

        Assert.Equivalent((string[])["Comedy", "Romance"], anime.AnimeGenres.Select(ag => ag.Genre.Name));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyGenres_WhenAnimeHasNoGenres()
    {
        var options = CreateOptions();

        await SeedAsync(options, CreateAnime("Steins;Gate", 9.07m));

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Empty(Assert.Single(result).AnimeGenres);
    }

    [Fact]
    public async Task GetAllAsync_LoadsOnlyOwnGenresForEachAnime()
    {
        var options = CreateOptions();

        var comedy = CreateGenre("Comedy");

        var drama = CreateGenre("Drama");

        await SeedAsync(
            options,
            CreateAnimeWithGenres("Sousou no Frieren", 9.24m, drama),
            CreateAnimeWithGenres("Kaguya-sama", 8.40m, comedy)
        );

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Equal(["Drama"], result[0].AnimeGenres.Select(ag => ag.Genre.Name));

        Assert.Equal(["Comedy"], result[1].AnimeGenres.Select(ag => ag.Genre.Name));
    }

    [Fact]
    public async Task GetAllAsync_LoadsCharacters()
    {
        var options = CreateOptions();

        var kaguya = CreateCharacter("Kaguya");

        var miyuki = CreateCharacter("Miyuki");

        await SeedAsync(options, CreateAnimeWithCharacters("Kaguya-sama", 8.40m, kaguya, miyuki));

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        var anime = Assert.Single(result);

        Assert.Equivalent((string[])["Kaguya", "Miyuki"], anime.AnimeCharacters.Select(ac => ac.Character.Name));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyCharacters_WhenAnimeHasNoCharacters()
    {
        var options = CreateOptions();

        await SeedAsync(options, CreateAnime("Steins;Gate", 9.07m));

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Empty(Assert.Single(result).AnimeCharacters);
    }

    [Fact]
    public async Task GetAllAsync_LoadsOnlyOwnCharactersForEachAnime()
    {
        var options = CreateOptions();

        var frieren = CreateCharacter("Frieren");

        var kaguya = CreateCharacter("Kaguya");

        await SeedAsync(
            options,
            CreateAnimeWithCharacters("Sousou no Frieren", 9.24m, frieren),
            CreateAnimeWithCharacters("Kaguya-sama", 8.40m, kaguya)
        );

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Equal(["Frieren"], result[0].AnimeCharacters.Select(ac => ac.Character.Name));

        Assert.Equal(["Kaguya"], result[1].AnimeCharacters.Select(ac => ac.Character.Name));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull()
    {
        await using var context = new AppDbContext(CreateOptions());

        var repository = new AnimeRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAnime()
    {
        var options = CreateOptions();

        var anime = CreateAnime("Steins;Gate", 9.07m);

        await SeedAsync(options, anime);

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        Assert.Equal(anime.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_LoadsGenres()
    {
        var options = CreateOptions();

        var action = CreateGenre("Action");

        var fantasy = CreateGenre("Fantasy");

        var anime = CreateAnimeWithGenres("Chainsaw Man", 8.41m, action, fantasy);

        await SeedAsync(options, anime);

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        Assert.Equivalent((string[])["Action", "Fantasy"], result.AnimeGenres.Select(ag => ag.Genre.Name));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmptyGenres_WhenAnimeHasNoGenres()
    {
        var options = CreateOptions();

        var anime = CreateAnime("Steins;Gate", 9.07m);

        await SeedAsync(options, anime);

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        Assert.Empty(result.AnimeGenres);
    }

    [Fact]
    public async Task GetByIdAsync_LoadsCharacters()
    {
        var options = CreateOptions();

        var denji = CreateCharacter("Denji");

        var power = CreateCharacter("Power");

        var anime = CreateAnimeWithCharacters("Chainsaw Man", 8.41m, denji, power);

        await SeedAsync(options, anime);

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        Assert.Equivalent((string[])["Denji", "Power"], result.AnimeCharacters.Select(ac => ac.Character.Name));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmptyCharacters_WhenAnimeHasNoCharacters()
    {
        var options = CreateOptions();

        var anime = CreateAnime("Steins;Gate", 9.07m);

        await SeedAsync(options, anime);

        await using var context = new AppDbContext(options);

        var repository = new AnimeRepository(context);

        var result = await repository.GetByIdAsync(anime.Id);

        Assert.NotNull(result);

        Assert.Empty(result.AnimeCharacters);
    }
}
