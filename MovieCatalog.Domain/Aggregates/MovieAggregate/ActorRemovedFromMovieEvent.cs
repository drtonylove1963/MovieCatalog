namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Common;

public class ActorRemovedFromMovieEvent : DomainEvent
{
    public Guid MovieId { get; }
    public Guid ActorId { get; }

    public ActorRemovedFromMovieEvent(Guid movieId, Guid actorId)
    {
        MovieId = movieId;
        ActorId = actorId;
    }
}