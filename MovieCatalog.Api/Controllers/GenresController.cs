namespace MovieCatalog.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Domain.Aggregates.GenreAggregate;
using MovieCatalog.Domain.Repositories;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreRepository _genreRepository;

    public GenresController(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
    {
        var genres = await _genreRepository.GetAllAsync();
        var result = genres.Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetGenre(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        
        if (genre == null)
            return NotFound();

        var result = new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateGenre([FromBody] CreateGenreDto dto)
    {
        var genreId = Guid.NewGuid();
        var genre = new Genre(
            genreId,
            dto.Name,
            dto.Description);

        await _genreRepository.AddAsync(genre);

        return CreatedAtAction(nameof(GetGenre), new { id = genreId }, genreId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGenre(Guid id, [FromBody] UpdateGenreDto dto)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        
        if (genre == null)
            return NotFound();

        var updatedGenre = new Genre(
            id,
            dto.Name,
            dto.Description);

        await _genreRepository.UpdateAsync(updatedGenre);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        
        if (genre == null)
            return NotFound();

        await _genreRepository.DeleteAsync(id);

        return NoContent();
    }
}

public class GenreDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class CreateGenreDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class UpdateGenreDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}