namespace MovieCatalog.Infrastructure.Persistence;

using System;

public class MovieActorReadModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}