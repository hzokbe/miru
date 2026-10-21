using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class MangaCharacterConfiguration : IEntityTypeConfiguration<MangaCharacter>
{
    public void Configure(EntityTypeBuilder<MangaCharacter> builder)
    {
        builder.HasKey(mc => new { mc.MangaId, mc.CharacterId });

        builder.Property(mc => mc.MangaId).HasColumnName("manga_id");

        builder.Property(mc => mc.CharacterId).HasColumnName("character_id");

        builder.HasOne(mc => mc.Manga).WithMany(m => m.MangaCharacters).HasForeignKey(mc => mc.MangaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mc => mc.Character).WithMany(c => c.MangaCharacters).HasForeignKey(mc => mc.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
