namespace MovieCatalog.Infrastructure.EventHandlers;

using Wolverine;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;
using Wolverine.Attributes;

// Handle MovieUpdatedEvent
public class MovieUpdatedHandler
{
    [WolverineHandler]
    public static async Task Handle(MovieUpdatedEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new MovieUpdatedNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record MovieUpdatedNotification(MovieUpdatedEvent Event);