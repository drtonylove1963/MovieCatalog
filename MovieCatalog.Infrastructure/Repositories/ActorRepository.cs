namespace MovieCatalog.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MovieCatalog.Domain.Aggregates.ActorAggregate;
using MovieCatalog.Domain.Repositories;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ActorRepository : IActorRepository
{
    private readonly WriteDbContext _dbContext;

    public ActorRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Actor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var actorEntity = await _dbContext.Actors
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (actorEntity == null)
            return null;

        return new Actor(
            actorEntity.Id,
            actorEntity.Name,
            actorEntity.DateOfBirth,
            actorEntity.Biography);
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var actorEntities = await _dbContext.Actors.ToListAsync(cancellationToken);

        return actorEntities.Select(a => new Actor(
            a.Id,
            a.Name,
            a.DateOfBirth,
            a.Biography));
    }

    public async Task AddAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        var actorEntity = new ActorWriteModel
        {
            Id = actor.Id,
            Name = actor.Name,
            DateOfBirth = actor.DateOfBirth,
            Biography = actor.Biography
        };

        await _dbContext.Actors.AddAsync(actorEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        var actorEntity = await _dbContext.Actors.FindAsync(new object[] { actor.Id }, cancellationToken);
        
        if (actorEntity == null)
            return;

        actorEntity.Name = actor.Name;
        actorEntity.DateOfBirth = actor.DateOfBirth;
        actorEntity.Biography = actor.Biography;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var actorEntity = await _dbContext.Actors.FindAsync(new object[] { id }, cancellationToken);
        
        if (actorEntity != null)
        {
            _dbContext.Actors.Remove(actorEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}