using System;

namespace MovieCatalog.Application.Actors.Queries.Common;

public class ActorListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}
