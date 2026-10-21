using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(32).IsRequired();

        builder.Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(16);

        builder.Property(c => c.JapaneseName).HasColumnName("japanese_name").HasMaxLength(8);

        builder.Property(c => c.JapaneseLastName).HasColumnName("japanese_last_name").HasMaxLength(8);

        builder.Property(c => c.Age).HasColumnName("age").HasMaxLength(8);

        builder.Property(c => c.Gender).HasColumnName("gender").HasConversion<string>().HasMaxLength(7).IsRequired();

        builder.Property(c => c.Role).HasColumnName("role").HasConversion<string>().HasMaxLength(10).IsRequired();

        builder.Property(c => c.Description).HasColumnName("description");

        builder.Property(c => c.ImageSource).HasColumnName("image_source");
    }
}
