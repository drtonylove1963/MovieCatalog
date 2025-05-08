using MediatR;
using MovieCatalog.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Genres.Commands.DeleteGenre;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
{
    private readonly IGenreRepository _genreRepository;

    public DeleteGenreCommandHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var existingGenre = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (existingGenre == null)
        {
            // Consider throwing a custom NotFoundException here
            return;
        }

        await _genreRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
