# Movie Catalog API

A comprehensive API for managing a movie catalog with actors, genres, and ratings, built using modern .NET technologies and clean architecture principles.

## Project Overview

The Movie Catalog application is a sophisticated backend system designed to manage a comprehensive movie database. It follows a Domain-Driven Design (DDD) approach with a Clean Architecture pattern, separating concerns into distinct layers. The system is built using a Command Query Responsibility Segregation (CQRS) pattern, which separates read and write operations for improved performance and scalability.

## Architecture

### Clean Architecture

The project follows Clean Architecture principles with clear separation of concerns:

1. **Domain Layer** - Contains the core business logic and entities
2. **Application Layer** - Contains application services, commands, and queries
3. **Infrastructure Layer** - Contains implementation details like databases and external services
4. **API Layer** - Exposes the functionality through RESTful endpoints

### CQRS Pattern

The application implements the Command Query Responsibility Segregation pattern:

- **Commands** - Handle write operations (create, update, delete)
- **Queries** - Handle read operations (get, list, search)

This separation allows for optimized data access patterns and improved scalability.

### Event Sourcing

The system is designed with event sourcing capabilities (temporarily disabled):
- Events are stored as the source of truth
- Domain events are raised when state changes occur
- Projections create read models from events

### Database Strategy

The application uses a dual-database approach:
- **SQL Server** - Used for write operations (command side)
- **PostgreSQL** - Used for read operations via Marten (query side, temporarily disabled)

### Messaging

RabbitMQ is integrated for asynchronous processing (temporarily disabled):
- Domain events are published to RabbitMQ
- Event handlers process the events asynchronously
- Wolverine is used for message handling and routing

## Domain Model

### Core Entities

1. **Movie**
   - Aggregate root with properties like Title, Description, ReleaseYear, DurationInMinutes, Rating
   - Contains collections of Actors and Genres
   - Raises domain events when state changes

2. **Actor**
   - Entity with properties like Name, DateOfBirth, Biography
   - Can be associated with multiple Movies

3. **Genre**
   - Entity with properties like Name, Description
   - Can be associated with multiple Movies

### Domain Events

The system uses domain events to track changes:
- MovieCreatedEvent
- MovieUpdatedEvent
- MovieRatingUpdatedEvent
- ActorAddedToMovieEvent
- ActorRemovedFromMovieEvent
- GenreAddedToMovieEvent
- GenreRemovedFromMovieEvent

## Application Layer

### Commands

Commands represent user intentions to change the system state:

1. **Movie Commands**
   - CreateMovieCommand
   - UpdateMovieCommand
   - UpdateMovieRatingCommand
   - AddActorToMovieCommand
   - AddGenreToMovieCommand

2. **Command Handlers**
   - Implement business logic
   - Validate input
   - Interact with repositories
   - Raise domain events

### Queries

Queries retrieve data from the system:

1. **Movie Queries**
   - GetMovieByIdQuery
   - GetMoviesListQuery (with pagination, search, and filtering)

2. **Query Handlers**
   - Retrieve data from read repositories
   - Map to DTOs for API responses

### Common Behaviors

The application implements cross-cutting concerns:
- Validation behavior for commands
- Logging behavior for all requests
- Exception handling behavior

## Infrastructure Layer

### Repositories

Repositories provide an abstraction over data access:
- MovieRepository (write)
- ActorRepository (write)
- GenreRepository (write)
- MovieReadRepository (read)

### Database Contexts

1. **WriteDbContext**
   - Entity Framework Core context for SQL Server
   - Handles write operations

2. **Marten Document Store**
   - PostgreSQL-based document store (temporarily disabled)
   - Handles read operations and event sourcing

### Event Handlers

Event handlers process domain events:
- Update read models
- Trigger side effects
- Maintain consistency between databases

## API Layer

### Controllers

RESTful controllers expose the application functionality:

1. **MoviesController**
   - GET /api/movies - List movies with pagination, search, and filtering
   - GET /api/movies/{id} - Get movie by ID
   - POST /api/movies - Create a new movie
   - PUT /api/movies/{id} - Update movie details
   - PUT /api/movies/{id}/rating - Update movie rating
   - POST /api/movies/{id}/actors - Add actor to movie
   - POST /api/movies/{id}/genres - Add genre to movie

2. **ActorsController**
   - GET /api/actors - List actors
   - GET /api/actors/{id} - Get actor by ID
   - POST /api/actors - Create a new actor

3. **GenresController**
   - GET /api/genres - List genres
   - GET /api/genres/{id} - Get genre by ID
   - POST /api/genres - Create a new genre

### DTOs

Data Transfer Objects define the API contract:
- CreateMovieCommand
- UpdateMovieDto
- MovieDto
- MovieListItemDto
- ActorDto
- GenreDto

## Technologies Used

- **.NET 8.0** - Latest version of the .NET platform
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for SQL Server
- **Marten** - Document DB and Event Store for PostgreSQL
- **MediatR** - Mediator pattern implementation for CQRS
- **Wolverine** - Message processing and routing
- **RabbitMQ** - Message broker for asynchronous processing
- **SQL Server** - Relational database for write operations
- **PostgreSQL** - Database for read operations and event store
- **Docker** - Containerization for development and deployment

## Development Setup

### Prerequisites

- .NET 8.0 SDK
- Docker Desktop
- Git

### Setup Steps

1. Clone the repository
2. Start the required Docker containers:
   ```
   docker-compose up -d
   ```
3. Run the application:
   ```
   dotnet run --project MovieCatalog.Api/MovieCatalog.Api.csproj
   ```
4. Access the API:
   - API Endpoints: http://localhost:5251/api
   - Swagger Documentation: http://localhost:5251/swagger/index.html

## Project Structure

```
MovieCatalog/
├── MovieCatalog.Api/              # API Layer
│   ├── Controllers/               # API Controllers
│   ├── Dtos/                      # Data Transfer Objects
│   └── Program.cs                 # Application entry point
├── MovieCatalog.Application/      # Application Layer
│   ├── Common/                    # Cross-cutting concerns
│   │   ├── Behaviors/             # Pipeline behaviors
│   │   ├── Interfaces/            # Application interfaces
│   │   └── Models/                # Application models
│   ├── Movies/                    # Movie feature
│   │   ├── Commands/              # Write operations
│   │   └── Queries/               # Read operations
│   └── DependencyInjection.cs     # Application services registration
├── MovieCatalog.Domain/           # Domain Layer
│   ├── Aggregates/                # Domain aggregates
│   │   ├── ActorAggregate/        # Actor entity and events
│   │   ├── GenreAggregate/        # Genre entity and events
│   │   └── MovieAggregate/        # Movie aggregate and events
│   ├── Common/                    # Domain base classes
│   └── Repositories/              # Repository interfaces
└── MovieCatalog.Infrastructure/   # Infrastructure Layer
    ├── EventHandlers/             # Domain event handlers
    ├── Persistence/               # Database contexts
    ├── Repositories/              # Repository implementations
    └── DependencyInjection.cs     # Infrastructure services registration
```

## Current Status

The project is currently in development with some features temporarily disabled for troubleshooting:
- Event sourcing with Marten is disabled
- RabbitMQ messaging is disabled
- PostgreSQL read database is disabled

The core functionality using SQL Server for both read and write operations is working.

## Future Enhancements

- Re-enable event sourcing with Marten
- Re-enable asynchronous processing with RabbitMQ
- Add authentication and authorization
- Implement more advanced search and filtering capabilities
- Add caching for improved performance
- Develop a frontend application
