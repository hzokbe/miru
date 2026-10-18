using Microsoft.EntityFrameworkCore;
using Miru.Models;

namespace Miru.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Anime> Animes => Set<Anime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.Property(a => a.Type).HasConversion<string>();

            entity.Property(a => a.Status).HasConversion<string>();

            entity.Property(a => a.Score).HasPrecision(4, 2);
        });
    }
}
