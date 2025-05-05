namespace MovieCatalog.Api.Dtos;

public class UpdateMovieDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int ReleaseYear { get; set; }
    public int DurationInMinutes { get; set; }
}