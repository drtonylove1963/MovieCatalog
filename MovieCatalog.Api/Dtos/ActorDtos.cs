namespace MovieCatalog.Api.Dtos;

using System;

public class ActorResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Biography { get; set; }
}

public class CreateActorDto
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Biography { get; set; }
}

public class UpdateActorDto
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Biography { get; set; }
}
