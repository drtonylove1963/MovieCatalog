using MediatR;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Genres.Queries.Common;

namespace MovieCatalog.Application.Genres.Queries.GetGenresList;

public class GetGenresListQuery : IRequest<PaginatedList<GenreListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}
