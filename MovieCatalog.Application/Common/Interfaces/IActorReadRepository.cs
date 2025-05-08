using MovieCatalog.Application.Actors.Queries.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Common.Interfaces;

public interface IActorReadRepository
{
    Task<ActorDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ActorListItemDto[]> GetAllAsync(CancellationToken cancellationToken);
}
