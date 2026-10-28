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

    [Fact]
    public async Task GetAllAsync_IncludesCharactersOfEachAnime()
    {
        var animes = AnimeSamples.CreateAll();

        var characters = CharacterSamples.CreateAll();

        var links = AnimeCharacterSamples.CreateAll(animes, characters);

        await _database.SeedAsync(animes);

        await using (var seedContext = _database.CreateContext())
        {
            seedContext.AddRange(characters);

            seedContext.AddRange(links);

            await seedContext.SaveChangesAsync();
        }

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetAllAsync();

        foreach (var anime in result)
        {
            var expected = links.Where(l => l.AnimeId == anime.Id).Select(l => l.CharacterId);

            Assert.Equal(expected.Order(), anime.AnimeCharacters.Select(ac => ac.CharacterId).Order());

            Assert.All(anime.AnimeCharacters, ac => Assert.NotNull(ac.Character));
        }
    }

    [Fact]
    public async Task GetByIdAsync_IncludesCharacters()
    {
        var animes = AnimeSamples.CreateAll();

        var characters = CharacterSamples.CreateAll();

        var links = AnimeCharacterSamples.CreateAll(animes, characters);

        var target = animes.First(a => links.Any(l => l.AnimeId == a.Id));

        await _database.SeedAsync(animes);

        await using (var seedContext = _database.CreateContext())
        {
            seedContext.AddRange(characters);

            seedContext.AddRange(links);

            await seedContext.SaveChangesAsync();
        }

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetByIdAsync(target.Id);

        Assert.NotNull(result);

        var expected = links.Where(l => l.AnimeId == target.Id).Select(l => l.CharacterId);

        Assert.Equal(expected.Order(), result.AnimeCharacters.Select(ac => ac.CharacterId).Order());

        Assert.All(result.AnimeCharacters, ac => Assert.NotNull(ac.Character));
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasNoCharacters_ReturnsEmptyAnimeCharacters()
    {
        var animes = AnimeSamples.CreateAll();

        await _database.SeedAsync(animes);

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetByIdAsync(animes[0].Id);

        Assert.NotNull(result);

        Assert.Empty(result.AnimeCharacters);
    }

    [Fact]
    public async Task SearchAsync_WhenNothingMatches_ReturnsEmptyList()
    {
        await _database.SeedAsync(AnimeSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).SearchAsync("zzzzzz");

        Assert.Empty(result);
    }

    [Theory]
    [InlineData("frieren")]
    [InlineData("FRIEREN")]
    [InlineData("Kaguya")]
    [InlineData("steins")]
    public async Task SearchAsync_ReturnsOnlyMatchingAnimesIgnoringCase(string query)
    {
        var animes = AnimeSamples.CreateAll();

        await _database.SeedAsync(animes);

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).SearchAsync(query);

        var expected = animes
            .Where(a => a.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(a => a.Score)
            .Select(a => a.Id);

        Assert.NotEmpty(result);

        Assert.Equal(expected, result.Select(a => a.Id));
    }

    [Fact]
    public async Task SearchAsync_ReturnsAnimesOrderedByScoreDescending()
    {
        await _database.SeedAsync(AnimeSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).SearchAsync("kaguya");

        Assert.Equal(3, result.Count);

        Assert.Equal(result.OrderByDescending(a => a.Score).Select(a => a.Id), result.Select(a => a.Id));
    }

    [Fact]
    public async Task SearchAsync_DoesNotTrackEntities()
    {
        await _database.SeedAsync(AnimeSamples.CreateAll());

        await using var context = _database.CreateContext();

        await new AnimeRepository(context).SearchAsync("frieren");

        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetAllAsync_IncludesGenresOfEachAnime()
    {
        var animes = AnimeSamples.CreateAll();

        var genres = GenreSamples.CreateAll();

        var links = AnimeGenreSamples.CreateAll(animes, genres);

        await _database.SeedAsync(animes);

        await using (var seedContext = _database.CreateContext())
        {
            seedContext.AddRange(genres);

            seedContext.AddRange(links);

            await seedContext.SaveChangesAsync();
        }

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetAllAsync();

        foreach (var anime in result)
        {
            var expected = links.Where(l => l.AnimeId == anime.Id).Select(l => l.GenreId);

            Assert.Equal(expected.Order(), anime.AnimeGenres.Select(ag => ag.GenreId).Order());

            Assert.All(anime.AnimeGenres, ag => Assert.NotNull(ag.Genre));
        }
    }

    [Fact]
    public async Task GetByIdAsync_IncludesGenres()
    {
        var animes = AnimeSamples.CreateAll();

        var genres = GenreSamples.CreateAll();

        var links = AnimeGenreSamples.CreateAll(animes, genres);

        var target = animes.First(a => links.Any(l => l.AnimeId == a.Id));

        await _database.SeedAsync(animes);

        await using (var seedContext = _database.CreateContext())
        {
            seedContext.AddRange(genres);

            seedContext.AddRange(links);

            await seedContext.SaveChangesAsync();
        }

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetByIdAsync(target.Id);

        Assert.NotNull(result);

        var expected = links.Where(l => l.AnimeId == target.Id).Select(l => l.GenreId);

        Assert.Equal(expected.Order(), result.AnimeGenres.Select(ag => ag.GenreId).Order());

        Assert.All(result.AnimeGenres, ag => Assert.NotNull(ag.Genre));
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeHasNoGenres_ReturnsEmptyAnimeGenres()
    {
        var animes = AnimeSamples.CreateAll();

        await _database.SeedAsync(animes);

        await using var context = _database.CreateContext();

        var result = await new AnimeRepository(context).GetByIdAsync(animes[0].Id);

        Assert.NotNull(result);

        Assert.Empty(result.AnimeGenres);
    }
}
