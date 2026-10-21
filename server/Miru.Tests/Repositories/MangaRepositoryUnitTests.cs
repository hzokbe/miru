using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;
using Miru.Repositories;

namespace Miru.Tests.Repositories;

public class MangaRepositoryUnitTests
{
    private static DbContextOptions<AppDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
    }

    private static async Task SeedAsync(DbContextOptions<AppDbContext> options, params Manga[] mangas)
    {
        await using var context = new AppDbContext(options);

        context.Mangas.AddRange(mangas);

        await context.SaveChangesAsync();
    }

    private static Manga CreateManga(string title, decimal? score)
    {
        return new Manga
        {
            Id = Guid.NewGuid(),
            Title = title,
            Score = score
        };
    }

    private static Genre CreateGenre(string name)
    {
        return new Genre { Id = Guid.NewGuid(), Name = name };
    }

    private static Manga CreateMangaWithGenres(string title, decimal? score, params Genre[] genres)
    {
        var manga = CreateManga(title, score);

        manga.MangaGenres = [.. genres.Select(g => new MangaGenre { MangaId = manga.Id, GenreId = g.Id, Genre = g })];

        return manga;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenThereAreNoMangas()
    {
        await using var context = new AppDbContext(CreateOptions());

        var repository = new MangaRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMangasOrderedByScore()
    {
        var options = CreateOptions();

        await SeedAsync(
            options,
            CreateManga("Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen", 8.88m),
            CreateManga("Yotsuba to!", 8.92m),
            CreateManga("Kaoru Hana wa Rin to Saku", 8.76m)
        );

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetAllAsync();

        string[] expected =
        [
            "Yotsuba to!",
            "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            "Kaoru Hana wa Rin to Saku"
        ];

        Assert.Equal(expected, result.Select(m => m.Title));
    }

    [Fact]
    public async Task GetAllAsync_LoadsGenres()
    {
        var options = CreateOptions();

        var drama = CreateGenre("Drama");

        var sliceOfLife = CreateGenre("Slice of Life");

        await SeedAsync(options, CreateMangaWithGenres("Oyasumi Punpun", 8.97m, drama, sliceOfLife));

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetAllAsync();

        var manga = Assert.Single(result);

        Assert.Equivalent((string[])["Drama", "Slice of Life"], manga.MangaGenres.Select(mg => mg.Genre.Name));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyGenres_WhenMangaHasNoGenres()
    {
        var options = CreateOptions();

        await SeedAsync(options, CreateManga("Yotsuba to!", 8.92m));

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Empty(Assert.Single(result).MangaGenres);
    }

    [Fact]
    public async Task GetAllAsync_LoadsOnlyOwnGenresForEachManga()
    {
        var options = CreateOptions();

        var comedy = CreateGenre("Comedy");

        var drama = CreateGenre("Drama");

        await SeedAsync(
            options,
            CreateMangaWithGenres("Oyasumi Punpun", 8.97m, drama),
            CreateMangaWithGenres("Yotsuba to!", 8.92m, comedy)
        );

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Equal(["Drama"], result[0].MangaGenres.Select(mg => mg.Genre.Name));

        Assert.Equal(["Comedy"], result[1].MangaGenres.Select(mg => mg.Genre.Name));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull()
    {
        await using var context = new AppDbContext(CreateOptions());

        var repository = new MangaRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsManga()
    {
        var options = CreateOptions();

        var manga = CreateManga("Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen", 8.88m);

        await SeedAsync(options, manga);

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Equal(manga.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_LoadsGenres()
    {
        var options = CreateOptions();

        var action = CreateGenre("Action");

        var adventure = CreateGenre("Adventure");

        var manga = CreateMangaWithGenres("Vagabond", 9.27m, action, adventure);

        await SeedAsync(options, manga);

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Equivalent((string[])["Action", "Adventure"], result.MangaGenres.Select(mg => mg.Genre.Name));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmptyGenres_WhenMangaHasNoGenres()
    {
        var options = CreateOptions();

        var manga = CreateManga("Yotsuba to!", 8.92m);

        await SeedAsync(options, manga);

        await using var context = new AppDbContext(options);

        var repository = new MangaRepository(context);

        var result = await repository.GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Empty(result.MangaGenres);
    }
}
