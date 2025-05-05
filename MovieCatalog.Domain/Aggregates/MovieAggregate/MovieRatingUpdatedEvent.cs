namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Common;

public class MovieRatingUpdatedEvent : DomainEvent
{
    public Guid MovieId { get; }
    public double Rating { get; }

    public MovieRatingUpdatedEvent(Guid movieId, double rating)
    {
        MovieId = movieId;
        Rating = rating;
    }
}
