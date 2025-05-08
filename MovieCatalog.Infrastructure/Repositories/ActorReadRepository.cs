using Marten;
using Marten.Linq;
using MovieCatalog.Application.Actors.Queries.Common;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Infrastructure.Repositories;

public class ActorReadRepository : IActorReadRepository
{
    private readonly IQuerySession _querySession;

    public ActorReadRepository(IQuerySession querySession)
    {
        _querySession = querySession;
    }

    public async Task<ActorDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var actorReadModel = await _querySession
            .Query<ActorReadModel>()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (actorReadModel == null)
            return null!; // Using null-forgiving operator as we're checking for null

        return new ActorDto
        {
            Id = actorReadModel.Id,
            Name = actorReadModel.Name,
            DateOfBirth = actorReadModel.DateOfBirth,
            Biography = actorReadModel.Biography,
            MovieIds = actorReadModel.MovieIds
        };
    }

    public async Task<ActorListItemDto[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var actors = await _querySession
            .Query<ActorReadModel>()
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return actors.Select(a => new ActorListItemDto
        {
            Id = a.Id,
            Name = a.Name,
            DateOfBirth = a.DateOfBirth
        }).ToArray();
    }
}
