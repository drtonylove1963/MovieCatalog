using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using MovieCatalog.Application;
using MovieCatalog.Infrastructure;
using MovieCatalog.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application services
builder.Services.AddApplication();

// Add infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Apply database migrations and seed data during development
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var dbContext = services.GetRequiredService<WriteDbContext>();
            dbContext.Database.EnsureCreated();
            
            // Seed the database with initial data if it's empty
            if (!dbContext.Movies.Any() && !dbContext.Actors.Any() && !dbContext.Genres.Any())
            {
                logger.LogInformation("Database is empty. Seeding data...");
                
                // Create genres
                var action = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Action", Description = "Action-packed movies with high-energy scenes and characters" };
                var comedy = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Comedy", Description = "Movies intended to make the audience laugh" };
                var drama = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Drama", Description = "Character-driven plots focusing on realistic situations" };
                var sciFi = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Science Fiction", Description = "Speculative fiction with futuristic concepts" };
                var horror = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Horror", Description = "Movies designed to frighten and scare the audience" };
                var romance = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Romance", Description = "Focus on the romantic relationships between characters" };
                var thriller = new GenreWriteModel { Id = Guid.NewGuid(), Name = "Thriller", Description = "Suspenseful movies designed to excite and engage" };
                
                dbContext.Genres.AddRange(action, comedy, drama, sciFi, horror, romance, thriller);
                dbContext.SaveChanges();
                
                // Create actors
                var tomHanks = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Tom Hanks", DateOfBirth = new DateTime(1956, 7, 9), Biography = "American actor and filmmaker, known for both comedic and dramatic roles." };
                var merylStreep = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Meryl Streep", DateOfBirth = new DateTime(1949, 6, 22), Biography = "American actress often described as the best actress of her generation." };
                var leonardoDiCaprio = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Leonardo DiCaprio", DateOfBirth = new DateTime(1974, 11, 11), Biography = "American actor known for his unconventional roles, particularly in biopics and period films." };
                var violaDavis = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Viola Davis", DateOfBirth = new DateTime(1965, 8, 11), Biography = "American actress and producer, the only African-American to achieve the Triple Crown of Acting." };
                var robertDowneyJr = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Robert Downey Jr.", DateOfBirth = new DateTime(1965, 4, 4), Biography = "American actor known for his roles in Marvel movies as Iron Man." };
                var jenniferLawrence = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Jennifer Lawrence", DateOfBirth = new DateTime(1990, 8, 15), Biography = "American actress known for her roles in The Hunger Games and Silver Linings Playbook." };
                var denzelWashington = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Denzel Washington", DateOfBirth = new DateTime(1954, 12, 28), Biography = "American actor, director, and producer, known for his intense performances." };
                var emmaStone = new ActorWriteModel { Id = Guid.NewGuid(), Name = "Emma Stone", DateOfBirth = new DateTime(1988, 11, 6), Biography = "American actress known for her roles in La La Land and The Favourite." };
                
                dbContext.Actors.AddRange(tomHanks, merylStreep, leonardoDiCaprio, violaDavis, robertDowneyJr, jenniferLawrence, denzelWashington, emmaStone);
                dbContext.SaveChanges();
                
                // Create movies
                var shawshank = new MovieWriteModel { 
                    Id = Guid.NewGuid(),
                    Title = "The Shawshank Redemption", 
                    Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    ReleaseYear = 1994,
                    DurationInMinutes = 142,
                    Rating = 9.3,
                    Version = 1
                };
                
                var godfather = new MovieWriteModel { 
                    Id = Guid.NewGuid(),
                    Title = "The Godfather", 
                    Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    ReleaseYear = 1972,
                    DurationInMinutes = 175,
                    Rating = 9.2,
                    Version = 1
                };
                
                var inception = new MovieWriteModel { 
                    Id = Guid.NewGuid(),
                    Title = "Inception", 
                    Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                    ReleaseYear = 2010,
                    DurationInMinutes = 148,
                    Rating = 8.8,
                    Version = 1
                };
                
                var forrestGump = new MovieWriteModel { 
                    Id = Guid.NewGuid(),
                    Title = "Forrest Gump", 
                    Description = "The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other historical events unfold from the perspective of an Alabama man with an IQ of 75.",
                    ReleaseYear = 1994,
                    DurationInMinutes = 142,
                    Rating = 8.8,
                    Version = 1
                };
                
                var darkKnight = new MovieWriteModel { 
                    Id = Guid.NewGuid(),
                    Title = "The Dark Knight", 
                    Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                    ReleaseYear = 2008,
                    DurationInMinutes = 152,
                    Rating = 9.0,
                    Version = 1
                };
                
                dbContext.Movies.AddRange(shawshank, godfather, inception, forrestGump, darkKnight);
                dbContext.SaveChanges();
                
                // Associate actors with movies
                shawshank.Actors.Add(denzelWashington);
                shawshank.Actors.Add(tomHanks);
                
                godfather.Actors.Add(leonardoDiCaprio);
                godfather.Actors.Add(robertDowneyJr);
                
                inception.Actors.Add(leonardoDiCaprio);
                inception.Actors.Add(emmaStone);
                
                forrestGump.Actors.Add(tomHanks);
                forrestGump.Actors.Add(merylStreep);
                
                darkKnight.Actors.Add(robertDowneyJr);
                darkKnight.Actors.Add(jenniferLawrence);
                
                // Associate genres with movies
                shawshank.Genres.Add(drama);
                
                godfather.Genres.Add(drama);
                godfather.Genres.Add(thriller);
                
                inception.Genres.Add(action);
                inception.Genres.Add(sciFi);
                inception.Genres.Add(thriller);
                
                forrestGump.Genres.Add(drama);
                forrestGump.Genres.Add(comedy);
                forrestGump.Genres.Add(romance);
                
                darkKnight.Genres.Add(action);
                darkKnight.Genres.Add(drama);
                darkKnight.Genres.Add(thriller);
                
                dbContext.SaveChanges();
                
                logger.LogInformation("Seeding completed successfully.");
            }
            else
            {
                logger.LogInformation("Database already contains data. Skipping seed operation.");
            }
            
            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();