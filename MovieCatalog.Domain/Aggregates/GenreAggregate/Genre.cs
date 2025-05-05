namespace MovieCatalog.Domain.Aggregates.GenreAggregate;

using MovieCatalog.Domain.Common;
using System;

public class Genre : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Genre() : base() { }

    public Genre(Guid id, string name, string description)
        : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
}