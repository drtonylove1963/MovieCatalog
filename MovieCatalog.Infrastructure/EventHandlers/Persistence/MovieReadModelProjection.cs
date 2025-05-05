namespace MovieCatalog.Infrastructure.EventHandlers.Persistence;

using Marten;
using Marten.Events;
using Marten.Events.Projections;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class MovieReadModelProjection : EventProjection
{
    public MovieReadModel Create(MovieCreatedEvent @event)
    {
        return new MovieReadModel
        {
            Id = @event.MovieId,
            Title = @event.Title,
            Description = @event.Description,
            ReleaseYear = @event.ReleaseYear,
            DurationInMinutes = @event.DurationInMinutes,
            Rating = 0,
            Version = 1
        };
    }

    public async Task Project(MovieUpdatedEvent @event, IDocumentOperations operations)
    {
        var movie = await operations.LoadAsync<MovieReadModel>(@event.MovieId);
        if (movie != null)
        {
            movie.Title = @event.Title;
            movie.Description = @event.Description;
            movie.ReleaseYear = @event.ReleaseYear;
            movie.DurationInMinutes = @event.DurationInMinutes;
            movie.Version++;
            
            operations.Store(movie);
        }
    }

    public async Task Project(MovieRatingUpdatedEvent @event, IDocumentOperations operations)
    {
        var movie = await operations.LoadAsync<MovieReadModel>(@event.MovieId);
        if (movie != null)
        {
            movie.Rating = @event.Rating;
            movie.Version++;
            
            operations.Store(movie);
        }
    }

    public async Task Project(ActorAddedToMovieEvent @event, IDocumentOperations operations)
    {
        var movie = await operations.LoadAsync<MovieReadModel>(@event.MovieId);
        if (movie != null)
        {
            if (!movie.Actors.Any(a => a.Id == @event.ActorId))
            {
                movie.Actors.Add(new MovieActorReadModel
                {
                    Id = @event.ActorId,
                    Name = @event.ActorName
                });
            }
            
            movie.Version++;
            operations.Store(movie);
        }
    }

    public async Task Project(ActorRemovedFromMovieEvent @event, IDocumentOperations operations)
    {
        var movie = await operations.LoadAsync<MovieReadModel>(@event.MovieId);
        if (movie != null)
        {
            var actor = movie.Actors.FirstOrDefault(a => a.Id == @event.ActorId);
            
            if (actor != null)
            {
                movie.Actors.Remove(actor);
            }
            
            movie.Version++;
            operations.Store(movie);
        }
    }

    public async Task Project(GenreAddedToMovieEvent @event, IDocumentOperations operations)
    {
        var movie = await operations.LoadAsync<MovieReadModel>(@event.MovieId);
        if (movie != null)
        {
            if (!movie.Genres.Any(g => g.Id == @event.GenreId))
            {
                movie.Genres.Add(new MovieGenreReadModel
                {
                    Id = @event.GenreId,
                    Name = @event.GenreName
                });
            }
            
            movie.Version++;
            operations.Store(movie);
        }
    }

    public async Task Project(GenreRemovedFromMovieEvent @event, IDocumentOperations operations)
    {
        var movie = await operations.LoadAsync<MovieReadModel>(@event.MovieId);
        if (movie != null)
        {
            var genre = movie.Genres.FirstOrDefault(g => g.Id == @event.GenreId);
            
            if (genre != null)
            {
                movie.Genres.Remove(genre);
            }
            
            movie.Version++;
            operations.Store(movie);
        }
    }
}