using Marten;
using Marten.Linq;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Genres.Queries.Common;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Infrastructure.Repositories;

public class GenreReadRepository : IGenreReadRepository
{
    private readonly IQuerySession _querySession;

    public GenreReadRepository(IQuerySession querySession)
    {
        _querySession = querySession;
    }

    public async Task<GenreDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var genreReadModel = await _querySession
            .Query<GenreReadModel>()
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (genreReadModel == null)
            return null!; // Using null-forgiving operator as we're checking for null

        return new GenreDto
        {
            Id = genreReadModel.Id,
            Name = genreReadModel.Name,
            Description = genreReadModel.Description,
            MovieIds = genreReadModel.MovieIds
        };
    }

    public async Task<GenreListItemDto[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var genres = await _querySession
            .Query<GenreReadModel>()
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);

        return genres.Select(g => new GenreListItemDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        }).ToArray();
    }
}
