namespace MovieCatalog.Application.Movies.Queries.Common;

using System;
using System.Collections.Generic;

public class MovieDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public int DurationInMinutes { get; set; }
    public double Rating { get; set; }
    public List<MovieActorDto> Actors { get; set; } = new();
    public List<MovieGenreDto> Genres { get; set; } = new();
}
