using Microsoft.EntityFrameworkCore;
using Miru.Models;

namespace Miru.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Anime> Animes => Set<Anime>();

    public DbSet<Manga> Mangas => Set<Manga>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<AnimeGenre> AnimeGenres => Set<AnimeGenre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.Property(a => a.Type).HasConversion<string>();

            entity.Property(a => a.Status).HasConversion<string>();

            entity.Property(a => a.Score).HasPrecision(4, 2);
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.Property(m => m.Status).HasConversion<string>();

            entity.Property(m => m.Score).HasPrecision(4, 2);
        });

        modelBuilder.Entity<AnimeGenre>(entity =>
        {
            entity.HasKey(ag => new { ag.AnimeId, ag.GenreId });

            entity.HasOne(ag => ag.Anime).WithMany(a => a.AnimeGenres).HasForeignKey(ac => ac.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ag => ag.Genre).WithMany(c => c.AnimeGenres).HasForeignKey(ac => ac.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
