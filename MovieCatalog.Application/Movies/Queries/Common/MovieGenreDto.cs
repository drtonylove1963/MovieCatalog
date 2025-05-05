namespace MovieCatalog.Application.Movies.Queries.Common;

using System;

public class MovieGenreDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
