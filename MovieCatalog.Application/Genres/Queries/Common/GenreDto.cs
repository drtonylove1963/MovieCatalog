using System;
using System.Collections.Generic;

namespace MovieCatalog.Application.Genres.Queries.Common;

public class GenreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Guid> MovieIds { get; set; } = new List<Guid>();
}
