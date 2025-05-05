namespace MovieCatalog.Infrastructure.Persistence;

using System;
using System.Collections.Generic;

public class MovieWriteModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public int DurationInMinutes { get; set; }
    public double Rating { get; set; }
    public int Version { get; set; }

    public ICollection<ActorWriteModel> Actors { get; set; } = new HashSet<ActorWriteModel>();
    public ICollection<GenreWriteModel> Genres { get; set; } = new HashSet<GenreWriteModel>();
}