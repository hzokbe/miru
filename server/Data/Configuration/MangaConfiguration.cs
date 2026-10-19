using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class MangaConfiguration : IEntityTypeConfiguration<Manga>
{
    public void Configure(EntityTypeBuilder<Manga> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(m => m.Title).HasColumnName("title").HasMaxLength(256).IsRequired();

        builder.Property(m => m.JapaneseTitle).HasColumnName("japanese_title").HasMaxLength(256);

        builder.Property(m => m.Synopsis).HasColumnName("synopsis");

        builder.Property(m => m.Chapters).HasColumnName("chapters");

        builder.Property(m => m.Volumes).HasColumnName("volumes");

        builder.Property(m => m.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(15).IsRequired();

        builder.Property(m => m.PublishedFrom).HasColumnName("published_from");

        builder.Property(m => m.PublishedTo).HasColumnName("published_to");

        builder.Property(m => m.Score).HasColumnName("score").HasPrecision(4, 2);

        builder.Property(m => m.ImageSource).HasColumnName("image_source");
    }
}
