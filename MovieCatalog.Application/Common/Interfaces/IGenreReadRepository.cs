using MovieCatalog.Application.Genres.Queries.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Common.Interfaces;

public interface IGenreReadRepository
{
    Task<GenreDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<GenreListItemDto[]> GetAllAsync(CancellationToken cancellationToken);
}
