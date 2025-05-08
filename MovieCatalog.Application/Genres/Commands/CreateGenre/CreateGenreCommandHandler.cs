using MediatR;
using MovieCatalog.Domain.Aggregates.GenreAggregate;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Guid>
{
    private readonly IGenreRepository _genreRepository;

    public CreateGenreCommandHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Guid> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genreId = Guid.NewGuid();
        
        var genre = new Genre(
            genreId,
            request.Name,
            request.Description);

        await _genreRepository.AddAsync(genre, cancellationToken);

        return genreId;
    }
}
