using Microsoft.EntityFrameworkCore;
using Miru.Models;

namespace Miru.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Anime> Animes => Set<Anime>();

    public DbSet<Manga> Mangas => Set<Manga>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
