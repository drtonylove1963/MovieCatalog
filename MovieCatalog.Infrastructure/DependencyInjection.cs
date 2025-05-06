using Marten;
using Marten.Events.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using MovieCatalog.Domain.Repositories;
using MovieCatalog.Infrastructure.EventHandlers.Persistence;
using MovieCatalog.Infrastructure.Persistence;
using MovieCatalog.Infrastructure.Repositories;
using MovieCatalog.Infrastructure.Services;
using System;

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

        // Configure Marten (Read DB) with simplified configuration
        var postgresConnection = configuration.GetConnectionString("PostgreSqlConnection") 
            ?? throw new InvalidOperationException("PostgreSQL connection string 'PostgreSqlConnection' not found in configuration.");
        
        // Add Marten with a more resilient configuration
        services.AddMarten(options =>
        {
            // Configure PostgreSQL connection
            options.Connection(postgresConnection);
            
            // Set a reasonable command timeout
            options.CommandTimeout = 30;
            
            // Configure database schema name
            options.DatabaseSchemaName = "movie_catalog";
            
            // Configure Event Store
            options.Events.AddEventType<MovieCreatedEvent>();
            options.Events.AddEventType<MovieUpdatedEvent>();
            options.Events.AddEventType<MovieRatingUpdatedEvent>();
            options.Events.AddEventType<ActorAddedToMovieEvent>();
            options.Events.AddEventType<ActorRemovedFromMovieEvent>();
            options.Events.AddEventType<GenreAddedToMovieEvent>();
            options.Events.AddEventType<GenreRemovedFromMovieEvent>();
            
            // Configure Projections with inline mode for simplicity
            options.Projections.Add<MovieReadModelProjection>(ProjectionLifecycle.Inline);
            
            // Optimize for aggregate operations
            options.Projections.UseIdentityMapForAggregates = true;
            
            // Enable auto-create for development
            options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
        })
        .OptimizeArtifactWorkflow()
        .UseLightweightSessions();

        // Register repositories
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        
        // Use SQL Server implementation for read repository temporarily
        // until PostgreSQL connection issues are resolved
        services.AddScoped<IMovieReadRepository, TempMovieReadRepository>();
        
        // Register the EventStoreService with a conditional check
        services.AddHostedService<EventStoreService>();

        return services;
    }
}