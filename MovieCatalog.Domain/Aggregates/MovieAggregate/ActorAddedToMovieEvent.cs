namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Common;

public class ActorAddedToMovieEvent : DomainEvent
{
    public Guid MovieId { get; }
    public Guid ActorId { get; }
    public string ActorName { get; }

    public ActorAddedToMovieEvent(Guid movieId, Guid actorId, string actorName)
    {
        MovieId = movieId;
        ActorId = actorId;
        ActorName = actorName;
    }
}