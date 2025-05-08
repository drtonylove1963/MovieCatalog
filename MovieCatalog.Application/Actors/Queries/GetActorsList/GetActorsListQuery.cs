using MediatR;
using MovieCatalog.Application.Actors.Queries.Common;
using MovieCatalog.Application.Common.Models;

namespace MovieCatalog.Application.Actors.Queries.GetActorsList;

public class GetActorsListQuery : IRequest<PaginatedList<ActorListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}
