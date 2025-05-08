using MediatR;
using MovieCatalog.Application.Actors.Queries.Common;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Actors.Queries.GetActorsList;

public class GetActorsListQueryHandler : IRequestHandler<GetActorsListQuery, PaginatedList<ActorListItemDto>>
{
    private readonly IActorReadRepository _actorReadRepository;

    public GetActorsListQueryHandler(IActorReadRepository actorReadRepository)
    {
        _actorReadRepository = actorReadRepository;
    }

    public async Task<PaginatedList<ActorListItemDto>> Handle(GetActorsListQuery request, CancellationToken cancellationToken)
    {
        var actors = await _actorReadRepository.GetAllAsync(cancellationToken);
        
        // Apply search filter if provided
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            actors = actors.Where(a => 
                a.Name.Contains(request.SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        // Create paginated result
        var totalCount = actors.Length;
        var items = actors
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
            
        return new PaginatedList<ActorListItemDto>(
            items, 
            totalCount, 
            request.PageNumber, 
            request.PageSize);
    }
}
