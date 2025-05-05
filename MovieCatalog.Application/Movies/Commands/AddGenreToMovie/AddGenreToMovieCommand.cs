using MediatR;
using System;

namespace MovieCatalog.Application.Movies.Commands.AddGenreToMovie;

public class AddGenreToMovieCommand : IRequest
{
    public Guid MovieId { get; set; }
    public Guid GenreId { get; set; }
}
