namespace MovieCatalog.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCatalog.Infrastructure.Persistence;

public class ActorConfiguration : IEntityTypeConfiguration<ActorWriteModel>
{
    public void Configure(EntityTypeBuilder<ActorWriteModel> builder)
    {
        builder.ToTable("Actors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.DateOfBirth)
            .IsRequired();

        builder.Property(a => a.Biography)
            .HasMaxLength(5000);
    }
}