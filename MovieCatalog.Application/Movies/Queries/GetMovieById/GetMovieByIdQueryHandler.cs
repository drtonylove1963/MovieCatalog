using MediatR;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Movies.Queries.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Movies.Queries.GetMovieById;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
{
    private readonly IMovieReadRepository _movieReadRepository;

    public GetMovieByIdQueryHandler(IMovieReadRepository movieReadRepository)
    {
        _movieReadRepository = movieReadRepository;
    }

    public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        return await _movieReadRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
