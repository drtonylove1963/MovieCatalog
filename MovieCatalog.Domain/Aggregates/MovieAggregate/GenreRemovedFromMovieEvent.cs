namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Common;

public class GenreRemovedFromMovieEvent : DomainEvent
{
    public Guid MovieId { get; }
    public Guid GenreId { get; }

    public GenreRemovedFromMovieEvent(Guid movieId, Guid genreId)
    {
        MovieId = movieId;
        GenreId = genreId;
    }
}