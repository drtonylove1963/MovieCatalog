using Marten;
using Marten.Linq;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Queries.Common;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Temporarily modified to use Marten as per the architecture
public class MovieReadRepository : IMovieReadRepository
{
    private readonly IQuerySession _querySession;

    public MovieReadRepository(IQuerySession querySession)
    {
        _querySession = querySession;
    }

    public async Task<MovieDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movieReadModel = await _querySession
            .Query<MovieReadModel>()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (movieReadModel == null)
            return null!; // Using null-forgiving operator as we're checking for null

        return new MovieDto
        {
            Id = movieReadModel.Id,
            Title = movieReadModel.Title,
            Description = movieReadModel.Description,
            ReleaseYear = movieReadModel.ReleaseYear,
            DurationInMinutes = movieReadModel.DurationInMinutes,
            Rating = movieReadModel.Rating,
            Actors = movieReadModel.Actors.Select(a => new MovieActorDto
            {
                Id = a.Id,
                Name = a.Name
            }).ToList(),
            Genres = movieReadModel.Genres.Select(g => new MovieGenreDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList()
        };
    }

    public async Task<PaginatedList<MovieListItemDto>> GetMoviesListAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null, 
        Guid? genreId = null,
        CancellationToken cancellationToken = default)
    {
        // Start with base query - explicitly cast to IMartenQueryable
        var baseQuery = _querySession.Query<MovieReadModel>();
        
        // Build query with filters
        var filteredQuery = baseQuery;
        
        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            filteredQuery = (IMartenQueryable<MovieReadModel>)filteredQuery.Where(m => m.Title.ToLower().Contains(searchTerm));
        }

        // Apply genre filter if provided
        if (genreId.HasValue)
        {
            filteredQuery = (IMartenQueryable<MovieReadModel>)filteredQuery.Where(m => m.Genres.Any(g => g.Id == genreId.Value));
        }

        // Get total count
        var totalCount = await filteredQuery.CountAsync(cancellationToken);

        // Apply pagination and select projection
        var items = await filteredQuery
            .OrderByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var dtos = items.Select(m => new MovieListItemDto
        {
            Id = m.Id,
            Title = m.Title,
            ReleaseYear = m.ReleaseYear,
            Rating = m.Rating,
            Genres = m.Genres.Select(g => g.Name).ToList()
        }).ToList();

        return new PaginatedList<MovieListItemDto>(dtos, totalCount, pageNumber, pageSize);
    }
}