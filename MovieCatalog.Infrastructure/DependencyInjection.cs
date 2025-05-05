using Marten;
using Marten.Events.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using MovieCatalog.Domain.Repositories;
using MovieCatalog.Infrastructure.EventHandlers.Persistence;
using MovieCatalog.Infrastructure.Persistence;
using MovieCatalog.Infrastructure.Repositories;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Marten;
using Wolverine.RabbitMQ;

namespace MovieCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure SQL Server (Write DB)
        services.AddDbContext<WriteDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServerConnection"),
                b => b.MigrationsAssembly(typeof(WriteDbContext).Assembly.FullName)));

        // TEMPORARILY DISABLED FOR TROUBLESHOOTING
        /*
        // Configure Marten (Read DB)
        services.AddMarten(options =>
        {
            // Configure PostgreSQL connection
            var postgresConnection = configuration.GetConnectionString("PostgreSqlConnection") 
                ?? throw new InvalidOperationException("PostgreSQL connection string 'PostgreSqlConnection' not found in configuration.");
            options.Connection(postgresConnection);
            options.DatabaseSchemaName = "movie_catalog";
            
            // Configure Event Store
            options.Events.AddEventType<MovieCreatedEvent>();
            options.Events.AddEventType<MovieUpdatedEvent>();
            options.Events.AddEventType<MovieRatingUpdatedEvent>();
            options.Events.AddEventType<ActorAddedToMovieEvent>();
            options.Events.AddEventType<ActorRemovedFromMovieEvent>();
            options.Events.AddEventType<GenreAddedToMovieEvent>();
            options.Events.AddEventType<GenreRemovedFromMovieEvent>();
            
            // Configure Projections
            options.Projections.Add<MovieReadModelProjection>(ProjectionLifecycle.Inline);
            
            // Optimize for aggregate operations
            options.Projections.UseIdentityMapForAggregates = true;
        })
        .UseLightweightSessions()
        .IntegrateWithWolverine();

        // Configure Wolverine
        services.AddWolverine(options =>
        {
            // Configure RabbitMQ
            var rabbitMqConnection = configuration.GetConnectionString("RabbitMqConnection") ?? "localhost";
            options.UseRabbitMq(rabbitMqConnection)
                .AutoProvision()
                // Configure topic exchange with bindings for routing keys
                .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-created-queue", "movie.created")
                    .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-updated-queue", "movie.updated")
                    .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-rating-queue", "movie.rating")
                    .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-actor-added-queue", "movie.actor.added")
                    .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-actor-removed-queue", "movie.actor.removed")
                    .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-genre-added-queue", "movie.genre.added")
                    .BindExchange("movie-events", ExchangeType.Topic)
                    .ToQueue("movie-genre-removed-queue", "movie.genre.removed");
            
            // Configure listeners for the queues
            options.ListenToRabbitQueue("movie-created-queue");
            options.ListenToRabbitQueue("movie-updated-queue");
            options.ListenToRabbitQueue("movie-rating-queue");
            options.ListenToRabbitQueue("movie-actor-added-queue");
            options.ListenToRabbitQueue("movie-actor-removed-queue");
            options.ListenToRabbitQueue("movie-genre-added-queue");
            options.ListenToRabbitQueue("movie-genre-removed-queue");
            
            // Configure message publishing
            options.PublishAllMessages().ToRabbitExchange("movie-events");
            
            // Retry policies
            options.Policies.OnException<Exception>()
                .RetryWithCooldown(
                    TimeSpan.FromMilliseconds(50), 
                    TimeSpan.FromMilliseconds(100), 
                    TimeSpan.FromMilliseconds(250));
        });
        */

        // Register repositories
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        
        // TEMPORARILY USING SQL SERVER IMPLEMENTATION FOR TROUBLESHOOTING
        services.AddScoped<IMovieReadRepository, TempMovieReadRepository>();

        return services;
    }
}