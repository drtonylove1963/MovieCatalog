using MediatR;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Queries.Common;

namespace MovieCatalog.Application.Movies.Queries.GetMoviesList;

public class GetMoviesListQuery : IRequest<PaginatedList<MovieListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? GenreId { get; set; }
}
