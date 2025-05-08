using MediatR;
using MovieCatalog.Application.Genres.Queries.Common;
using System;

namespace MovieCatalog.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQuery : IRequest<GenreDto>
{
    public Guid Id { get; set; }
}
