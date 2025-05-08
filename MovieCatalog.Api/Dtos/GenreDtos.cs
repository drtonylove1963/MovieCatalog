namespace MovieCatalog.Api.Dtos;

using System;

public class GenreDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class CreateGenreDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class UpdateGenreDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}
