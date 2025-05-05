namespace MovieCatalog.Infrastructure.Persistence;

using System;
using System.Collections.Generic;

public class ActorWriteModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Biography { get; set; }

    public ICollection<MovieWriteModel> Movies { get; set; } = new HashSet<MovieWriteModel>();
}