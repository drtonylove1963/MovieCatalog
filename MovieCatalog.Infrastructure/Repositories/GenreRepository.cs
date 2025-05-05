namespace MovieCatalog.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MovieCatalog.Domain.Aggregates.GenreAggregate;
using MovieCatalog.Domain.Repositories;
using MovieCatalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GenreRepository : IGenreRepository
{
    private readonly WriteDbContext _dbContext;

    public GenreRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var genreEntity = await _dbContext.Genres
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (genreEntity == null)
            return null;

        return new Genre(
            genreEntity.Id,
            genreEntity.Name,
            genreEntity.Description);
    }

    public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var genreEntities = await _dbContext.Genres.ToListAsync(cancellationToken);

        return genreEntities.Select(g => new Genre(
            g.Id,
            g.Name,
            g.Description));
    }

    public async Task AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        var genreEntity = new GenreWriteModel
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };

        await _dbContext.Genres.AddAsync(genreEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        var genreEntity = await _dbContext.Genres.FindAsync(new object[] { genre.Id }, cancellationToken);
        
        if (genreEntity == null)
            return;

        genreEntity.Name = genre.Name;
        genreEntity.Description = genre.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var genreEntity = await _dbContext.Genres.FindAsync(new object[] { id }, cancellationToken);
        
        if (genreEntity != null)
        {
            _dbContext.Genres.Remove(genreEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}