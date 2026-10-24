using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;

namespace Miru.Tests.Support;

public sealed class InMemoryDatabaseFixture
{
    private readonly DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

    public AppDbContext CreateContext()
    {
        return new AppDbContext(_options);
    }

    public async Task SeedAsync(IEnumerable<Anime> animes)
    {
        await using var context = CreateContext();

        context.Animes.AddRange(animes);

        await context.SaveChangesAsync();
    }

    public async Task SeedAsync(IEnumerable<Manga> mangas)
    {
        await using var context = CreateContext();

        context.Mangas.AddRange(mangas);

        await context.SaveChangesAsync();
    }
}
