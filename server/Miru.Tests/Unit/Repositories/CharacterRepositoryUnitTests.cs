using Miru.Data.Seeding;
using Miru.Models;
using Miru.Repositories;
using Miru.Tests.Support;

namespace Miru.Tests.Unit.Repositories;

public class CharacterRepositoryUnitTests
{
    private readonly InMemoryDatabaseFixture _database = new();

    private async Task SeedAsync(IEnumerable<Character> characters)
    {
        await using var context = _database.CreateContext();

        context.AddRange(characters);

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WhenDatabaseIsEmpty_ReturnsNull()
    {
        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCharacterDoesNotExist_ReturnsNull()
    {
        await SeedAsync(CharacterSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCharacterExists_ReturnsMatchingCharacter()
    {
        var characters = CharacterSamples.CreateAll();

        await SeedAsync(characters);

        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).GetByIdAsync(characters[0].Id);

        Assert.NotNull(result);

        Assert.Equal(characters[0].Id, result.Id);

        Assert.Equal(characters[0].Name, result.Name);

        Assert.Equal(characters[0].LastName, result.LastName);

        Assert.Equal(characters[0].JapaneseName, result.JapaneseName);

        Assert.Equal(characters[0].JapaneseLastName, result.JapaneseLastName);

        Assert.Equal(characters[0].Age, result.Age);

        Assert.Equal(characters[0].Gender, result.Gender);

        Assert.Equal(characters[0].Role, result.Role);

        Assert.Equal(characters[0].Description, result.Description);

        Assert.Equal(characters[0].ImageSource, result.ImageSource);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOnlyRequestedCharacter()
    {
        var characters = CharacterSamples.CreateAll();

        await SeedAsync(characters);

        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).GetByIdAsync(characters[^1].Id);

        Assert.NotNull(result);

        Assert.Equal(characters[^1].Id, result.Id);
    }

    [Fact]
    public async Task SearchAsync_WhenNothingMatches_ReturnsEmptyList()
    {
        await _database.SeedAsync(CharacterSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).SearchAsync("zzzzzz");

        Assert.Empty(result);
    }

    [Theory]
    [InlineData("frieren")]
    [InlineData("RINTAROU")]
    [InlineData("edward")]
    [InlineData("pun")]
    public async Task SearchAsync_ReturnsOnlyMatchingCharactersIgnoringCase(string query)
    {
        var characters = CharacterSamples.CreateAll();

        await _database.SeedAsync(characters);

        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).SearchAsync(query);

        var expected = characters
            .Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Id);

        Assert.NotEmpty(result);

        Assert.Equal(expected.Order(), result.Select(c => c.Id).Order());
    }

    [Fact]
    public async Task SearchAsync_WhenNameIsShared_ReturnsAllCharactersWithThatName()
    {
        await _database.SeedAsync(CharacterSamples.CreateAll());

        await using var context = _database.CreateContext();

        var result = await new CharacterRepository(context).SearchAsync("Rintarou");

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchAsync_DoesNotTrackEntities()
    {
        await _database.SeedAsync(CharacterSamples.CreateAll());

        await using var context = _database.CreateContext();

        await new CharacterRepository(context).SearchAsync("frieren");

        Assert.Empty(context.ChangeTracker.Entries());
    }
}
