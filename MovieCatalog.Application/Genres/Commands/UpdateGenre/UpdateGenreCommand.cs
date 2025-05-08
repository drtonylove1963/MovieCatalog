using MediatR;
using System;

namespace MovieCatalog.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
