using Wolverine;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;
using Wolverine.Attributes;

namespace MovieCatalog.Infrastructure.EventHandlers;

// Handle MovieCreatedEvent
public class MovieCreatedHandler
{
    [WolverineHandler]
    public static async Task Handle(MovieCreatedEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new MovieCreatedNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record MovieCreatedNotification(MovieCreatedEvent Event);
