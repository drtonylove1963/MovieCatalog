namespace MovieCatalog.Application.Movies.Queries.Common;

using System;

public class MovieActorDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
