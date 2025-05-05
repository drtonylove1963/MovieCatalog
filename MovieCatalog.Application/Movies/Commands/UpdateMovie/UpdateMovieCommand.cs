using MediatR;
using System;

namespace MovieCatalog.Application.Movies.Commands.UpdateMovie;

public class UpdateMovieCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public int DurationInMinutes { get; set; }
}
