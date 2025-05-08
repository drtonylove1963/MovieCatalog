namespace MovieCatalog.Infrastructure.Persistence;

using System;
using System.Collections.Generic;

public class ActorReadModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Biography { get; set; }
    public List<Guid> MovieIds { get; set; } = new List<Guid>();
    public int Version { get; set; }
}
