namespace MovieCatalog.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCatalog.Infrastructure.Persistence;

public class MovieConfiguration : IEntityTypeConfiguration<MovieWriteModel>
{
    public void Configure(EntityTypeBuilder<MovieWriteModel> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);

        builder.Property(m => m.ReleaseYear)
            .IsRequired();

        builder.Property(m => m.DurationInMinutes)
            .IsRequired();

        builder.Property(m => m.Rating)
            .HasPrecision(3, 1)
            .HasDefaultValue(0);

        builder.Property(m => m.Version)
            .IsRequired()
            .IsConcurrencyToken();

        // Many-to-many relationship with actors
        builder
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            .UsingEntity<Dictionary<string, object>>(
                "MovieActors",
                j => j
                    .HasOne<ActorWriteModel>()
                    .WithMany()
                    .HasForeignKey("ActorId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<MovieWriteModel>()
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Cascade));

        // Many-to-many relationship with genres
        builder
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity<Dictionary<string, object>>(
                "MovieGenres",
                j => j
                    .HasOne<GenreWriteModel>()
                    .WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<MovieWriteModel>()
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}