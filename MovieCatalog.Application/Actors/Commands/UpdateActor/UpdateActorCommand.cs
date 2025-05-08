using MediatR;
using System;

namespace MovieCatalog.Application.Actors.Commands.UpdateActor;

public class UpdateActorCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Biography { get; set; } = string.Empty;
}
