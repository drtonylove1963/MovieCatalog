using MediatR;
using MovieCatalog.Domain.Aggregates.GenreAggregate;
using MovieCatalog.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand>
{
    private readonly IGenreRepository _genreRepository;

    public UpdateGenreCommandHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var existingGenre = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (existingGenre == null)
        {
            // Consider throwing a custom NotFoundException here
            return;
        }

        var updatedGenre = new Genre(
            request.Id,
            request.Name,
            request.Description);

        await _genreRepository.UpdateAsync(updatedGenre, cancellationToken);
    }
}
