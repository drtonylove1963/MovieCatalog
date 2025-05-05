namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Common;

public class MovieUpdatedEvent : DomainEvent
{
    public Guid MovieId { get; }
    public string Title { get; }
    public string Description { get; }
    public int ReleaseYear { get; }
    public int DurationInMinutes { get; }

    public MovieUpdatedEvent(Guid movieId, string title, string description, int releaseYear, int durationInMinutes)
    {
        MovieId = movieId;
        Title = title;
        Description = description;
        ReleaseYear = releaseYear;
        DurationInMinutes = durationInMinutes;
    }
}