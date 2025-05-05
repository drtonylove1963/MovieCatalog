using MediatR;
using System;

namespace MovieCatalog.Application.Movies.Commands.UpdateMovieRating;

public class UpdateMovieRatingCommand : IRequest
{
    public Guid MovieId { get; set; }
    public double Rating { get; set; }
}
