namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Common;

public class GenreAddedToMovieEvent : DomainEvent
{
    public Guid MovieId { get; }
    public Guid GenreId { get; }
    public string GenreName { get; }

    public GenreAddedToMovieEvent(Guid movieId, Guid genreId, string genreName)
    {
        MovieId = movieId;
        GenreId = genreId;
        GenreName = genreName;
    }
}