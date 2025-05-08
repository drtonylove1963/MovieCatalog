using MediatR;
using System;

namespace MovieCatalog.Application.Actors.Commands.DeleteActor;

public class DeleteActorCommand : IRequest
{
    public Guid Id { get; set; }
}
