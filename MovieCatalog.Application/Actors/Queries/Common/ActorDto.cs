using System;
using System.Collections.Generic;

namespace MovieCatalog.Application.Actors.Queries.Common;

public class ActorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Biography { get; set; } = string.Empty;
    public List<Guid> MovieIds { get; set; } = new List<Guid>();
}
