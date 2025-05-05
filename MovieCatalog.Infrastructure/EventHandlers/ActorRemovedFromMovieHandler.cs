namespace MovieCatalog.Infrastructure.EventHandlers;

using Wolverine;
using Wolverine.Attributes;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;

// Handle ActorRemovedFromMovieEvent
public class ActorRemovedFromMovieHandler
{
    [WolverineHandler]
    public static async Task Handle(ActorRemovedFromMovieEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new ActorRemovedFromMovieNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record ActorRemovedFromMovieNotification(ActorRemovedFromMovieEvent Event);