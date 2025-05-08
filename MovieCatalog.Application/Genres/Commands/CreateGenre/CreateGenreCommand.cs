using MediatR;
using System;

namespace MovieCatalog.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
