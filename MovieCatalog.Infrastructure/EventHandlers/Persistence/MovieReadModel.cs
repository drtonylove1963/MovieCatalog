namespace MovieCatalog.Infrastructure.Persistence;

using Marten.Events.Projections;
using MovieCatalog.Domain.Aggregates.MovieAggregate;

public class MovieReadModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public int DurationInMinutes { get; set; }
    public double Rating { get; set; }
    public List<MovieActorReadModel> Actors { get; set; } = new List<MovieActorReadModel>();
    public List<MovieGenreReadModel> Genres { get; set; } = new List<MovieGenreReadModel>();
    public int Version { get; set; }
}