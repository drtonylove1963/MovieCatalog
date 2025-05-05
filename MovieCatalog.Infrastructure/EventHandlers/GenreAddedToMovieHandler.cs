namespace MovieCatalog.Infrastructure.EventHandlers;

using Wolverine;
using Wolverine.Attributes;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;

// Handle GenreAddedToMovieEvent
public class GenreAddedToMovieHandler
{
    [WolverineHandler]
    public static async Task Handle(GenreAddedToMovieEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new GenreAddedToMovieNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record GenreAddedToMovieNotification(GenreAddedToMovieEvent Event);