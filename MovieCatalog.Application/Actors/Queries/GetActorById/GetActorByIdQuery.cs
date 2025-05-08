using MediatR;
using MovieCatalog.Application.Actors.Queries.Common;
using System;

namespace MovieCatalog.Application.Actors.Queries.GetActorById;

public class GetActorByIdQuery : IRequest<ActorDto>
{
    public Guid Id { get; set; }
}
