namespace MovieCatalog.Domain.Repositories;

using MovieCatalog.Domain.Aggregates.ActorAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IActorRepository
{
    Task<Actor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Actor actor, CancellationToken cancellationToken = default);
    Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}