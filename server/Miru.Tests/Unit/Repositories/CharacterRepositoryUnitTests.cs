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
}
