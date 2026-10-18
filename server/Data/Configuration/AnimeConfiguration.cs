using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class AnimeConfiguration : IEntityTypeConfiguration<Anime>
{
    public void Configure(EntityTypeBuilder<Anime> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(a => a.Title).HasColumnName("title").HasMaxLength(256).IsRequired();

        builder.Property(a => a.JapaneseTitle).HasColumnName("japanese_title").HasMaxLength(256);

        builder.Property(a => a.Synopsis).HasColumnName("synopsis");

        builder.Property(a => a.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(9).IsRequired();

        builder.Property(a => a.Episodes).HasColumnName("episodes");

        builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(15).IsRequired();

        builder.Property(a => a.AiredFrom).HasColumnName("aired_from");

        builder.Property(a => a.AiredTo).HasColumnName("aired_to");

        builder.Property(a => a.Score).HasColumnName("score").HasPrecision(4, 2);

        builder.Property(a => a.ImageSource).HasColumnName("image_source");
    }
}
