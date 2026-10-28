using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miru.Models;

namespace Miru.Data.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("genres");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name).HasColumnName("name").HasMaxLength(32).IsRequired();

        builder.Property(g => g.Description).HasColumnName("description");
    }
}
