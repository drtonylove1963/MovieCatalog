using MediatR;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Domain.Aggregates.MovieAggregate;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Movies.Commands.CreateMovie;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Guid>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IGenreRepository _genreRepository;

    public CreateMovieCommandHandler(
        IMovieRepository movieRepository,
        IActorRepository actorRepository,
        IGenreRepository genreRepository)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _genreRepository = genreRepository;
    }

    public async Task<Guid> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        // Create a new movie entity
        var movie = Movie.Create(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.ReleaseYear,
            request.DurationInMinutes);

        // Add actors to the movie
        foreach (var actorId in request.ActorIds)
        {
            var actor = await _actorRepository.GetByIdAsync(actorId, cancellationToken);
            if (actor != null)
            {
                movie.AddActor(actor);
            }
        }

        // Add genres to the movie
        foreach (var genreId in request.GenreIds)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId, cancellationToken);
            if (genre != null)
            {
                movie.AddGenre(genre);
            }
        }

        // Save the movie
        await _movieRepository.AddAsync(movie, cancellationToken);

        return movie.Id;
    }
}
