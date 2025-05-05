namespace MovieCatalog.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using MovieCatalog.Infrastructure.Persistence.Configurations;
using System.Reflection;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options)
        : base(options)
    {
    }

    public DbSet<MovieWriteModel> Movies { get; set; }
    public DbSet<ActorWriteModel> Actors { get; set; }
    public DbSet<GenreWriteModel> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}