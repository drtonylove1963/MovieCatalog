namespace MovieCatalog.Infrastructure.Persistence;

using System;
using System.Collections.Generic;

public class GenreWriteModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public ICollection<MovieWriteModel> Movies { get; set; } = new HashSet<MovieWriteModel>();
}
