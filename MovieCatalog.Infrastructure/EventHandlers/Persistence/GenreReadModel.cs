namespace MovieCatalog.Infrastructure.Persistence;

using System;
using System.Collections.Generic;

public class GenreReadModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<Guid> MovieIds { get; set; } = new List<Guid>();
    public int Version { get; set; }
}
