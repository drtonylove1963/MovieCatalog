using MediatR;
using System;

namespace MovieCatalog.Application.Genres.Commands.DeleteGenre;

public class DeleteGenreCommand : IRequest
{
    public Guid Id { get; set; }
}
