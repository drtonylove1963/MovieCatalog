using MediatR;
using System;

namespace MovieCatalog.Application.Movies.Commands.AddActorToMovie;

public class AddActorToMovieCommand : IRequest
{
    public Guid MovieId { get; set; }
    public Guid ActorId { get; set; }
}
