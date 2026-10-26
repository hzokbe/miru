using Miru.Data.Seeding;
using Miru.Repositories;
using Miru.Tests.Support;

namespace Miru.Tests.Unit.Repositories;

public class MangaRepositoryUnitTests
{
    private readonly InMemoryDatabaseFixture _database = new();

    [Fact]
    public async Task GetAllAsync_WhenThereAreNoMangas_ReturnsEmptyList()
    {
        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMangasOrderedByScoreDescending()
    {
        var mangas = MangaSamples.CreateAll();

        await _database.SeedAsync(mangas);

        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetAllAsync();

        var expected = mangas.OrderByDescending(m => m.Score).Select(m => m.Id);

        Assert.Equal(expected, result.Select(m => m.Id));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSeededMangas()
    {
        var mangas = MangaSamples.CreateAll();

        await _database.SeedAsync(mangas);

        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetAllAsync();

        Assert.Equal(mangas.Count, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_DoesNotTrackEntities()
    {
        await _database.SeedAsync(MangaSamples.CreateAll());

        await using var context = _database.CreateContext();

        await new MangaRepository(context).GetAllAsync();

        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaDoesNotExist_ReturnsNull()
    {
        await _database.SeedAsync(MangaSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaExists_ReturnsMatchingManga()
    {
        var mangas = MangaSamples.CreateAll();

        await _database.SeedAsync(mangas);

        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetByIdAsync(mangas[0].Id);

        Assert.NotNull(result);

        MangaAssert.Matches(mangas[0], result);
    }

    [Fact]
    public async Task GetAllAsync_IncludesCharacters()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var manga = MangaCharacterLinker.WithCharacters(MangaSamples.CreateAll()[0], characters);

        await _database.SeedAsync([manga]);

        await using var context = _database.CreateContext();

        var result = Assert.Single(await new MangaRepository(context).GetAllAsync());

        Assert.Equal(
            characters.Select(c => c.Id).Order(),
            result.MangaCharacters.Select(mc => mc.Character.Id).Order()
        );
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        var manga = MangaSamples.CreateAll()[0];

        manga.MangaCharacters = [];

        await _database.SeedAsync([manga]);

        await using var context = _database.CreateContext();

        var result = Assert.Single(await new MangaRepository(context).GetAllAsync());

        Assert.Empty(result.MangaCharacters);
    }

    [Fact]
    public async Task GetByIdAsync_IncludesCharacters()
    {
        var characters = CharacterSamples.CreateAll().Take(3).ToList();

        var manga = MangaCharacterLinker.WithCharacters(MangaSamples.CreateAll()[0], characters);

        await _database.SeedAsync([manga]);

        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Equal(
            characters.Select(c => c.Id).Order(),
            result.MangaCharacters.Select(mc => mc.Character.Id).Order()
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        var manga = MangaSamples.CreateAll()[0];

        manga.MangaCharacters = [];

        await _database.SeedAsync([manga]);

        await using var context = _database.CreateContext();

        var result = await new MangaRepository(context).GetByIdAsync(manga.Id);

        Assert.NotNull(result);

        Assert.Empty(result.MangaCharacters);
    }
}
