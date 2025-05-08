using MediatR;
using MovieCatalog.Application.Actors.Queries.Common;
using MovieCatalog.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Actors.Queries.GetActorById;

public class GetActorByIdQueryHandler : IRequestHandler<GetActorByIdQuery, ActorDto>
{
    private readonly IActorReadRepository _actorReadRepository;

    public GetActorByIdQueryHandler(IActorReadRepository actorReadRepository)
    {
        _actorReadRepository = actorReadRepository;
    }

    public async Task<ActorDto> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
    {
        return await _actorReadRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
