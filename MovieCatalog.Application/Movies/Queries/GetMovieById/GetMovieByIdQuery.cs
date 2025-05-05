using MediatR;
using MovieCatalog.Application.Movies.Queries.Common;

namespace MovieCatalog.Application.Movies.Queries.GetMovieById;

public class GetMovieByIdQuery : IRequest<MovieDto>
{
    public Guid Id { get; set; }
}
