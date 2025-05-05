namespace MovieCatalog.Application.Movies.Queries.Common;

using System;
using System.Collections.Generic;

public class MovieListItemDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
    public List<string> Genres { get; set; } = new();
}
