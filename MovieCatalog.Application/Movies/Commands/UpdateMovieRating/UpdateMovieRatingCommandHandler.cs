using MediatR;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Movies.Commands.UpdateMovieRating;

public class UpdateMovieRatingCommandHandler : IRequestHandler<UpdateMovieRatingCommand>
{
    private readonly IMovieRepository _movieRepository;

    public UpdateMovieRatingCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task Handle(UpdateMovieRatingCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null)
        {
            throw new Exception($"Movie with ID {request.MovieId} not found");
        }

        movie.UpdateRating(request.Rating);
        await _movieRepository.UpdateAsync(movie, cancellationToken);
    }
}
