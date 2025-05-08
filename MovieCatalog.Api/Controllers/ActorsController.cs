namespace MovieCatalog.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Api.Dtos;
using MovieCatalog.Application.Actors.Commands.CreateActor;
using MovieCatalog.Application.Actors.Commands.DeleteActor;
using MovieCatalog.Application.Actors.Commands.UpdateActor;
using MovieCatalog.Application.Actors.Queries.Common;
using MovieCatalog.Application.Actors.Queries.GetActorById;
using MovieCatalog.Application.Actors.Queries.GetActorsList;
using MovieCatalog.Application.Common.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ActorListItemDto>>> GetActors(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _mediator.Send(new GetActorsListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetActor(Guid id)
    {
        var result = await _mediator.Send(new GetActorByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateActor([FromBody] CreateActorDto dto)
    {
        var command = new CreateActorCommand
        {
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            Biography = dto.Biography
        };

        var actorId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetActor), new { id = actorId }, actorId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateActor(Guid id, [FromBody] UpdateActorDto dto)
    {
        var command = new UpdateActorCommand
        {
            Id = id,
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            Biography = dto.Biography
        };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActor(Guid id)
    {
        var command = new DeleteActorCommand
        {
            Id = id
        };

        await _mediator.Send(command);

        return NoContent();
    }
}