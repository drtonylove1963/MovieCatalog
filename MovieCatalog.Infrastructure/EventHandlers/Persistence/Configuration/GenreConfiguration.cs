namespace MovieCatalog.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCatalog.Infrastructure.Persistence;

public class GenreConfiguration : IEntityTypeConfiguration<GenreWriteModel>
{
    public void Configure(EntityTypeBuilder<GenreWriteModel> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(g => g.Description)
            .HasMaxLength(500);
    }
}