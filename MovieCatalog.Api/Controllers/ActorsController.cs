namespace MovieCatalog.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Api.Dtos;
using MovieCatalog.Domain.Aggregates.ActorAggregate;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IActorRepository _actorRepository;

    public ActorsController(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorResponseDto>>> GetActors()
    {
        var actors = await _actorRepository.GetAllAsync();
        var result = actors.Select(a => new ActorResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            DateOfBirth = a.DateOfBirth,
            Biography = a.Biography
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActorResponseDto>> GetActor(Guid id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        
        if (actor == null)
            return NotFound();

        var result = new ActorResponseDto
        {
            Id = actor.Id,
            Name = actor.Name,
            DateOfBirth = actor.DateOfBirth,
            Biography = actor.Biography
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateActor([FromBody] CreateActorDto dto)
    {
        var actorId = Guid.NewGuid();
        var actor = new Actor(
            actorId,
            dto.Name,
            dto.DateOfBirth,
            dto.Biography);

        await _actorRepository.AddAsync(actor);

        return CreatedAtAction(nameof(GetActor), new { id = actorId }, actorId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateActor(Guid id, [FromBody] UpdateActorDto dto)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        
        if (actor == null)
            return NotFound();

        var updatedActor = new Actor(
            id,
            dto.Name,
            dto.DateOfBirth,
            dto.Biography);

        await _actorRepository.UpdateAsync(updatedActor);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActor(Guid id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        
        if (actor == null)
            return NotFound();

        await _actorRepository.DeleteAsync(id);

        return NoContent();
    }
}