namespace MovieCatalog.Infrastructure.EventHandlers;

using Wolverine;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using System.Threading.Tasks;
using Wolverine.Attributes;

// Handle MovieRatingUpdatedEvent
public class MovieRatingUpdatedHandler
{
    [WolverineHandler]
    public static async Task Handle(MovieRatingUpdatedEvent @event, IMessageBus messageBus)
    {
        // Log the event
        // We will publish a message to RabbitMQ to notify other services
        await messageBus.PublishAsync(new MovieRatingUpdatedNotification(@event));
    }
}

// Notification DTO for RabbitMQ
public record MovieRatingUpdatedNotification(MovieRatingUpdatedEvent Event);