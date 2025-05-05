namespace MovieCatalog.Infrastructure.EventHandlers;

using Wolverine;
using Wolverine.Attributes;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;

// Handle GenreRemovedFromMovieEvent
public class GenreRemovedFromMovieHandler
{
    [WolverineHandler]
    public static async Task Handle(GenreRemovedFromMovieEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new GenreRemovedFromMovieNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record GenreRemovedFromMovieNotification(GenreRemovedFromMovieEvent Event);
