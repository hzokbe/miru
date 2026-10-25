using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class AnimeCharacterConfiguration : IEntityTypeConfiguration<AnimeCharacter>
{
    public void Configure(EntityTypeBuilder<AnimeCharacter> builder)
    {
        builder.ToTable("anime_characters");

        builder.HasKey(ac => new { ac.AnimeId, ac.CharacterId });

        builder.Property(ac => ac.AnimeId).HasColumnName("anime_id");

        builder.Property(ac => ac.CharacterId).HasColumnName("character_id");

        builder.HasOne(ac => ac.Anime).WithMany(a => a.AnimeCharacters)
            .HasForeignKey(ac => ac.AnimeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ac => ac.Character)
            .WithMany(c => c.AnimeCharacters).HasForeignKey(ac => ac.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
