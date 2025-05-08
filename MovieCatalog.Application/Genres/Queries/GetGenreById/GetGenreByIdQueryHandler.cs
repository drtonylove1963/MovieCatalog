using MediatR;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Genres.Queries.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    private readonly IGenreReadRepository _genreReadRepository;

    public GetGenreByIdQueryHandler(IGenreReadRepository genreReadRepository)
    {
        _genreReadRepository = genreReadRepository;
    }

    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        return await _genreReadRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
