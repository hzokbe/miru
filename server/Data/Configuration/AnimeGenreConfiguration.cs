using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class AnimeGenreConfiguration : IEntityTypeConfiguration<AnimeGenre>
{
    public void Configure(EntityTypeBuilder<AnimeGenre> builder)
    {
        builder.ToTable("anime_genres");

        builder.HasKey(ag => new { ag.AnimeId, ag.GenreId });

        builder.Property(ag => ag.AnimeId).HasColumnName("anime_id");

        builder.Property(ag => ag.GenreId).HasColumnName("genre_id");

        builder.HasOne(ag => ag.Anime)
            .WithMany(a => a.AnimeGenres)
            .HasForeignKey(ag => ag.AnimeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ag => ag.Genre)
            .WithMany(ag => ag.AnimeGenres).HasForeignKey(ag => ag.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
