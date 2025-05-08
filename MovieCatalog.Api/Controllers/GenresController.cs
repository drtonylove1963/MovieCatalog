namespace MovieCatalog.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Api.Dtos;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Genres.Commands.CreateGenre;
using MovieCatalog.Application.Genres.Commands.DeleteGenre;
using MovieCatalog.Application.Genres.Commands.UpdateGenre;
using MovieCatalog.Application.Genres.Queries.Common;
using MovieCatalog.Application.Genres.Queries.GetGenreById;
using MovieCatalog.Application.Genres.Queries.GetGenresList;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<GenreListItemDto>>> GetGenres(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _mediator.Send(new GetGenresListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieCatalog.Application.Genres.Queries.Common.GenreDto>> GetGenre(Guid id)
    {
        var result = await _mediator.Send(new GetGenreByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateGenre([FromBody] CreateGenreDto dto)
    {
        var command = new CreateGenreCommand
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var genreId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetGenre), new { id = genreId }, genreId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGenre(Guid id, [FromBody] UpdateGenreDto dto)
    {
        var command = new UpdateGenreCommand
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description
        };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(Guid id)
    {
        var command = new DeleteGenreCommand
        {
            Id = id
        };

        await _mediator.Send(command);

        return NoContent();
    }
}