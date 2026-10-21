using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class MangaGenreConfiguration : IEntityTypeConfiguration<MangaGenre>
{
    public void Configure(EntityTypeBuilder<MangaGenre> builder)
    {
        builder.HasKey(mg => new { mg.MangaId, mg.GenreId });

        builder.Property(mg => mg.MangaId).HasColumnName("manga_id");

        builder.Property(mg => mg.GenreId).HasColumnName("genre_id");

        builder.HasOne(mg => mg.Manga).WithMany(m => m.MangaGenres).HasForeignKey(mg => mg.MangaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mg => mg.Genre).WithMany(g => g.MangaGenres).HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
