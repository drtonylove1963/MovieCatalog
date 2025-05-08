using MediatR;
using MovieCatalog.Domain.Aggregates.ActorAggregate;
using MovieCatalog.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Actors.Commands.UpdateActor;

public class UpdateActorCommandHandler : IRequestHandler<UpdateActorCommand>
{
    private readonly IActorRepository _actorRepository;

    public UpdateActorCommandHandler(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    public async Task Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        var existingActor = await _actorRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (existingActor == null)
        {
            // Consider throwing a custom NotFoundException here
            return;
        }

        var updatedActor = new Actor(
            request.Id,
            request.Name,
            request.DateOfBirth,
            request.Biography);

        await _actorRepository.UpdateAsync(updatedActor, cancellationToken);
    }
}
