using MediatR;
using System;

namespace MovieCatalog.Application.Actors.Commands.CreateActor;

public class CreateActorCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Biography { get; set; } = string.Empty;
}
