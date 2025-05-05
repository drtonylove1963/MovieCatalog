using MediatR;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Movies.Commands.AddGenreToMovie;

public class AddGenreToMovieCommandHandler : IRequestHandler<AddGenreToMovieCommand>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;

    public AddGenreToMovieCommandHandler(
        IMovieRepository movieRepository,
        IGenreRepository genreRepository)
    {
        _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
    }

    public async Task Handle(AddGenreToMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null)
        {
            throw new Exception($"Movie with ID {request.MovieId} not found");
        }

        var genre = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken);
        if (genre == null)
        {
            throw new Exception($"Genre with ID {request.GenreId} not found");
        }

        movie.AddGenre(genre);
        await _movieRepository.UpdateAsync(movie, cancellationToken);
    }
}
