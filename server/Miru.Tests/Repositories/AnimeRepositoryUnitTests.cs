using Microsoft.EntityFrameworkCore;
using Miru.Data;
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
}
