namespace MovieCatalog.Domain.Aggregates.ActorAggregate;

using MovieCatalog.Domain.Common;
using System;

public class Actor : Entity
{
    public string Name { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string Biography { get; private set; } = string.Empty;

    private Actor() : base() { }

    public Actor(Guid id, string name, DateTime dateOfBirth, string biography)
        : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DateOfBirth = dateOfBirth;
        Biography = biography ?? throw new ArgumentNullException(nameof(biography));
    }
}