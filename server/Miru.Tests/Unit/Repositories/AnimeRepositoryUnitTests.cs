using Miru.Data.Seeding;
using Miru.Repositories;
using Miru.Tests.Support;

namespace Miru.Tests.Unit.Repositories;

public class AnimeRepositoryUnitTests
{
    private readonly InMemoryDatabaseFixture _database = new();

    [Fact]
    public async Task GetAllAsync_WhenThereAreNoAnimes_ReturnsEmptyList()
    {
        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAnimesOrderedByScoreDescending()
    {
        var animes = AnimeSamples.CreateAll();

        await _database.SeedAsync(animes);

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetAllAsync();

        var expected = animes.OrderByDescending(a => a.Score).Select(a => a.Id);

        Assert.Equal(expected, result.Select(a => a.Id));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSeededAnimes()
    {
        var animes = AnimeSamples.CreateAll();

        await _database.SeedAsync(animes);

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetAllAsync();

        Assert.Equal(animes.Count, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_DoesNotTrackEntities()
    {
        await _database.SeedAsync(AnimeSamples.CreateAll());

        await using var context = _database.CreateContext();

        await new AnimeRepository(context).GetAllAsync();

        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeDoesNotExist_ReturnsNull()
    {
        await _database.SeedAsync(AnimeSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeExists_ReturnsMatchingAnime()
    {
        var animes = AnimeSamples.CreateAll();

        await _database.SeedAsync(animes);

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetByIdAsync(animes[0].Id);

        Assert.NotNull(result);

        AnimeAssert.Matches(animes[0], result);
    }
}
