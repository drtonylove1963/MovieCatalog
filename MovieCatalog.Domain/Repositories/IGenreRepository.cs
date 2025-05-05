namespace MovieCatalog.Domain.Repositories;

using MovieCatalog.Domain.Aggregates.GenreAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Genre>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Genre genre, CancellationToken cancellationToken = default);
    Task UpdateAsync(Genre genre, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}