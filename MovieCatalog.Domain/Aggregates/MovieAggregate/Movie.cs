namespace MovieCatalog.Domain.Aggregates.MovieAggregate;

using MovieCatalog.Domain.Aggregates.ActorAggregate;
using MovieCatalog.Domain.Aggregates.GenreAggregate;
using MovieCatalog.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

public class Movie : AggregateRoot
{
    private readonly List<Actor> _actors = new();
    private readonly List<Genre> _genres = new();
    
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int ReleaseYear { get; private set; }
    public int DurationInMinutes { get; private set; }
    public double Rating { get; private set; }
    public IReadOnlyCollection<Actor> Actors => _actors.AsReadOnly();
    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();

    // This parameterless constructor is needed for ORM
    protected Movie() : base() { }
    
    private Movie(Guid id, string title, string description, int releaseYear, int durationInMinutes)
        : base(id)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ReleaseYear = releaseYear;
        DurationInMinutes = durationInMinutes;
        Rating = 0;
    }
    
    public static Movie Create(Guid id, string title, string description, int releaseYear, int durationInMinutes)
    {
        var movie = new Movie(id, title, description, releaseYear, durationInMinutes);
        movie.AddDomainEvent(new MovieCreatedEvent(id, title, description, releaseYear, durationInMinutes));
        movie.IncrementVersion();
        return movie;
    }
    
    public void UpdateDetails(string title, string description, int releaseYear, int durationInMinutes)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ReleaseYear = releaseYear;
        DurationInMinutes = durationInMinutes;
        
        AddDomainEvent(new MovieUpdatedEvent(Id, title, description, releaseYear, durationInMinutes));
        IncrementVersion();
    }
    
    public void AddActor(Actor actor)
    {
        if (_actors.Any(a => a.Id == actor.Id))
            return;
            
        _actors.Add(actor);
        AddDomainEvent(new ActorAddedToMovieEvent(Id, actor.Id, actor.Name));
        IncrementVersion();
    }
    
    public void RemoveActor(Guid actorId)
    {
        var actor = _actors.FirstOrDefault(a => a.Id == actorId);
        if (actor == null)
            return;
            
        _actors.Remove(actor);
        AddDomainEvent(new ActorRemovedFromMovieEvent(Id, actorId));
        IncrementVersion();
    }
    
    public void AddGenre(Genre genre)
    {
        if (_genres.Any(g => g.Id == genre.Id))
            return;
            
        _genres.Add(genre);
        AddDomainEvent(new GenreAddedToMovieEvent(Id, genre.Id, genre.Name));
        IncrementVersion();
    }
    
    public void RemoveGenre(Guid genreId)
    {
        var genre = _genres.FirstOrDefault(g => g.Id == genreId);
        if (genre == null)
            return;
            
        _genres.Remove(genre);
        AddDomainEvent(new GenreRemovedFromMovieEvent(Id, genreId));
        IncrementVersion();
    }
    
    public void UpdateRating(double rating)
    {
        if (rating < 0 || rating > 10)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 10");
            
        Rating = rating;
        AddDomainEvent(new MovieRatingUpdatedEvent(Id, rating));
        IncrementVersion();
    }
}