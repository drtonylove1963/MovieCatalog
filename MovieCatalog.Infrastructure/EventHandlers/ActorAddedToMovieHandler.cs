namespace MovieCatalog.Infrastructure.EventHandlers;

using Wolverine;
using Wolverine.Attributes;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;

// Handle ActorAddedToMovieEvent
public class ActorAddedToMovieHandler
{
    [WolverineHandler]
    public static async Task Handle(ActorAddedToMovieEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new ActorAddedToMovieNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record ActorAddedToMovieNotification(ActorAddedToMovieEvent Event);