using Microsoft.EntityFrameworkCore;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Queries.Common;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Infrastructure.Repositories;

// Temporary implementation for troubleshooting that uses SQL Server instead of PostgreSQL
public class TempMovieReadRepository : IMovieReadRepository
{
    private readonly WriteDbContext _dbContext;

    public TempMovieReadRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MovieDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movieEntity = await _dbContext.Movies
            .Include(m => m.Actors)
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (movieEntity == null)
            return null!; // Using null-forgiving operator as we're checking for null

        return new MovieDto
        {
            Id = movieEntity.Id,
            Title = movieEntity.Title,
            Description = movieEntity.Description,
            ReleaseYear = movieEntity.ReleaseYear,
            DurationInMinutes = movieEntity.DurationInMinutes,
            Rating = movieEntity.Rating,
            Actors = movieEntity.Actors.Select(a => new MovieActorDto
            {
                Id = a.Id,
                Name = a.Name
            }).ToList(),
            Genres = movieEntity.Genres.Select(g => new MovieGenreDto
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
        // Start with base query
        var baseQuery = _dbContext.Movies
            .Include(m => m.Genres)
            .AsQueryable();
        
        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            baseQuery = baseQuery.Where(m => m.Title.ToLower().Contains(searchTerm));
        }

        // Apply genre filter if provided
        if (genreId.HasValue)
        {
            baseQuery = baseQuery.Where(m => m.Genres.Any(g => g.Id == genreId.Value));
        }

        // Get total count
        var totalCount = await baseQuery.CountAsync(cancellationToken);

        // Apply pagination and select projection
        var items = await baseQuery
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
