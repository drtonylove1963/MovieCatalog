using MediatR;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Queries.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Movies.Queries.GetMoviesList;

public class GetMoviesListQueryHandler : IRequestHandler<GetMoviesListQuery, PaginatedList<MovieListItemDto>>
{
    private readonly IMovieReadRepository _movieReadRepository;

    public GetMoviesListQueryHandler(IMovieReadRepository movieReadRepository)
    {
        _movieReadRepository = movieReadRepository;
    }

    public async Task<PaginatedList<MovieListItemDto>> Handle(GetMoviesListQuery request, CancellationToken cancellationToken)
    {
        return await _movieReadRepository.GetMoviesListAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.GenreId,
            cancellationToken);
    }
}
