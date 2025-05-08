using MediatR;
using MovieCatalog.Domain.Aggregates.ActorAggregate;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Actors.Commands.CreateActor;

public class CreateActorCommandHandler : IRequestHandler<CreateActorCommand, Guid>
{
    private readonly IActorRepository _actorRepository;

    public CreateActorCommandHandler(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    public async Task<Guid> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        var actorId = Guid.NewGuid();
        
        var actor = new Actor(
            actorId,
            request.Name,
            request.DateOfBirth,
            request.Biography);

        await _actorRepository.AddAsync(actor, cancellationToken);

        return actorId;
    }
}
