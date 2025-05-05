using MediatR;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Movies.Commands.AddActorToMovie;

public class AddActorToMovieCommandHandler : IRequestHandler<AddActorToMovieCommand>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;

    public AddActorToMovieCommandHandler(
        IMovieRepository movieRepository,
        IActorRepository actorRepository)
    {
        _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        _actorRepository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
    }

    public async Task Handle(AddActorToMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null)
        {
            throw new Exception($"Movie with ID {request.MovieId} not found");
        }

        var actor = await _actorRepository.GetByIdAsync(request.ActorId, cancellationToken);
        if (actor == null)
        {
            throw new Exception($"Actor with ID {request.ActorId} not found");
        }

        movie.AddActor(actor);
        await _movieRepository.UpdateAsync(movie, cancellationToken);
    }
}
