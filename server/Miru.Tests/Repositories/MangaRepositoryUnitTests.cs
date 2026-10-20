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
        var manga = new Manga
        {
            Id = Guid.NewGuid(),
            Title = title,
            Score = score
        };

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

        Assert.Equal(expected, result.Select(a => a.Title));
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
}
