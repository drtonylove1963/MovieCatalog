using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Api.Dtos;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Movies.Commands.AddActorToMovie;
using MovieCatalog.Application.Movies.Commands.AddGenreToMovie;
using MovieCatalog.Application.Movies.Commands.CreateMovie;
using MovieCatalog.Application.Movies.Commands.UpdateMovie;
using MovieCatalog.Application.Movies.Commands.UpdateMovieRating;
using MovieCatalog.Application.Movies.Queries.Common;
using MovieCatalog.Application.Movies.Queries.GetMovieById;
using MovieCatalog.Application.Movies.Queries.GetMoviesList;


namespace MovieCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<MovieListItemDto>>> GetMovies(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? genreId = null)
    {
        var result = await _mediator.Send(new GetMoviesListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            GenreId = genreId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(Guid id)
    {
        var result = await _mediator.Send(new GetMovieByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateMovie([FromBody] CreateMovieCommand command)
    {
        var movieId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMovie), new { id = movieId }, movieId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMovie(Guid id, [FromBody] UpdateMovieDto dto)
    {
        var command = new UpdateMovieCommand
        {
            Id = id,
            Title = dto.Title,
            Description = dto.Description,
            ReleaseYear = dto.ReleaseYear,
            DurationInMinutes = dto.DurationInMinutes
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/rating")]
    public async Task<ActionResult> UpdateMovieRating(Guid id, [FromBody] UpdateMovieRatingDto dto)
    {
        var command = new UpdateMovieRatingCommand
        {
            MovieId = id,
            Rating = dto.Rating
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/actors")]
    public async Task<ActionResult> AddActorToMovie(Guid id, [FromBody] AddActorToMovieDto dto)
    {
        var command = new AddActorToMovieCommand
        {
            MovieId = id,
            ActorId = dto.ActorId
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/genres")]
    public async Task<ActionResult> AddGenreToMovie(Guid id, [FromBody] AddGenreToMovieDto dto)
    {
        var command = new AddGenreToMovieCommand
        {
            MovieId = id,
            GenreId = dto.GenreId
        };

        await _mediator.Send(command);
        return NoContent();
    }
}
