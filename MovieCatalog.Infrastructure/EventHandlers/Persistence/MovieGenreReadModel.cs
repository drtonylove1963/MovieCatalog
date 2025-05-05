namespace MovieCatalog.Infrastructure.Persistence;

using System;

public class MovieGenreReadModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}