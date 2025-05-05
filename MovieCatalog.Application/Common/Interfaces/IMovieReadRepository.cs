using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Queries.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Common.Interfaces;

public interface IMovieReadRepository
{
    Task<MovieDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<PaginatedList<MovieListItemDto>> GetMoviesListAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null, 
        Guid? genreId = null,
        CancellationToken cancellationToken = default);
}
