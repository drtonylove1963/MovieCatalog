using Marten;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Queries.GetMovieById;
using MovieCatalog.Application.Movies.Queries.GetMoviesList;
using MovieCatalog.Domain.Aggregates.ActorAggregate;
using MovieCatalog.Domain.Aggregates.GenreAggregate;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using MovieCatalog.Domain.Repositories;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly WriteDbContext _dbContext;
    private readonly IDocumentSession _documentSession;

    public MovieRepository(WriteDbContext dbContext, IDocumentSession documentSession)
    {
        _dbContext = dbContext;
        _documentSession = documentSession;
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Use EF Core to fetch the movie from SQL Server
        var movieEntity = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
            _dbContext.Movies
                .Include(m => m.Actors)
                .Include(m => m.Genres),
            m => m.Id == id, 
            cancellationToken);

        if (movieEntity == null)
            return null;

        // Create a domain entity from the database entity using the static Create method
        var movie = Movie.Create(
            movieEntity.Id,
            movieEntity.Title,
            movieEntity.Description,
            movieEntity.ReleaseYear,
            movieEntity.DurationInMinutes);
            
        // Set the rating
        movie.UpdateRating(movieEntity.Rating);

        // Add actors and genres
        foreach (var actor in movieEntity.Actors)
        {
            movie.AddActor(new Actor(
                actor.Id, 
                actor.Name, 
                actor.DateOfBirth, 
                actor.Biography ?? string.Empty));
        }

        foreach (var genre in movieEntity.Genres)
        {
            movie.AddGenre(new Genre(
                genre.Id, 
                genre.Name,
                genre.Description ?? string.Empty));
        }

        return movie;
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        // Add to SQL Server (Write DB)
        var movieEntity = new MovieWriteModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            DurationInMinutes = movie.DurationInMinutes,
            Rating = movie.Rating,
            Version = movie.Version
        };

        await _dbContext.Movies.AddAsync(movieEntity, cancellationToken);

        // Append events to Marten (Event Store)
        _documentSession.Events.Append(movie.Id, movie.DomainEvents);

        // Save SQL Server operation
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _documentSession.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        // Update SQL Server (Write DB)
        var movieEntity = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
            _dbContext.Movies
                .Include(m => m.Actors)
                .Include(m => m.Genres),
            m => m.Id == movie.Id, 
            cancellationToken);

        if (movieEntity != null)
        {
            // Update basic properties
            movieEntity.Title = movie.Title;
            movieEntity.Description = movie.Description;
            movieEntity.ReleaseYear = movie.ReleaseYear;
            movieEntity.DurationInMinutes = movie.DurationInMinutes;
            movieEntity.Rating = movie.Rating;
            movieEntity.Version = movie.Version;

            // Update actors
            var currentActorIds = movieEntity.Actors.Select(a => a.Id).ToHashSet();
            var newActorIds = movie.Actors.Select(a => a.Id).ToHashSet();
            
            // Add new actors
            var actorsToAdd = newActorIds.Except(currentActorIds).ToList();
            if (actorsToAdd.Any())
            {
                var actors = await EntityFrameworkQueryableExtensions.ToListAsync(
                    _dbContext.Actors.Where(a => actorsToAdd.Contains(a.Id)),
                    cancellationToken);
                
                foreach (var actor in actors)
                {
                    movieEntity.Actors.Add(actor);
                }
            }
            
            // Remove actors
            var actorsToRemove = currentActorIds.Except(newActorIds).ToList();
            if (actorsToRemove.Any())
            {
                foreach (var actorId in actorsToRemove)
                {
                    var actor = movieEntity.Actors.FirstOrDefault(a => a.Id == actorId);
                    if (actor != null)
                    {
                        movieEntity.Actors.Remove(actor);
                    }
                }
            }

            // Update genres
            var currentGenreIds = movieEntity.Genres.Select(g => g.Id).ToHashSet();
            var newGenreIds = movie.Genres.Select(g => g.Id).ToHashSet();
            
            // Add new genres
            var genresToAdd = newGenreIds.Except(currentGenreIds).ToList();
            if (genresToAdd.Any())
            {
                var genres = await EntityFrameworkQueryableExtensions.ToListAsync(
                    _dbContext.Genres.Where(g => genresToAdd.Contains(g.Id)),
                    cancellationToken);
                
                foreach (var genre in genres)
                {
                    movieEntity.Genres.Add(genre);
                }
            }
            
            // Remove genres
            var genresToRemove = currentGenreIds.Except(newGenreIds).ToList();
            if (genresToRemove.Any())
            {
                foreach (var genreId in genresToRemove)
                {
                    var genre = movieEntity.Genres.FirstOrDefault(g => g.Id == genreId);
                    if (genre != null)
                    {
                        movieEntity.Genres.Remove(genre);
                    }
                }
            }
        }

        // Append events to Marten (Event Store)
        _documentSession.Events.Append(movie.Id, movie.DomainEvents);

        // Save SQL Server operation
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _documentSession.SaveChangesAsync(cancellationToken);

        // Clear domain events after saving
        movie.ClearDomainEvents();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movieEntity = await _dbContext.Movies.FindAsync(new object[] { id }, cancellationToken);
        
        if (movieEntity != null)
        {
            _dbContext.Movies.Remove(movieEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}