using MediatR;
using System;
using System.Collections.Generic;

namespace MovieCatalog.Application.Movies.Commands.CreateMovie;

public class CreateMovieCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int DurationInMinutes { get; set; }
    public List<Guid> ActorIds { get; set; } = new List<Guid>();
    public List<Guid> GenreIds { get; set; } = new List<Guid>();
}
