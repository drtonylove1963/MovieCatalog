using MediatR;
using MovieCatalog.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Actors.Commands.DeleteActor;

public class DeleteActorCommandHandler : IRequestHandler<DeleteActorCommand>
{
    private readonly IActorRepository _actorRepository;

    public DeleteActorCommandHandler(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    public async Task Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var existingActor = await _actorRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (existingActor == null)
        {
            // Consider throwing a custom NotFoundException here
            return;
        }

        await _actorRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
