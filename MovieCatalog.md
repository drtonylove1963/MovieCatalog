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

The system implements event sourcing with Marten:
- Events are stored as the source of truth in PostgreSQL
- Domain events are raised when state changes occur
- Projections create read models from events
- Resilient error handling ensures application stability

### Database Strategy

The application uses a dual-database approach:
- **SQL Server** - Used for write operations (command side) and temporary read operations
- **PostgreSQL** - Used for event store and future read operations with Marten

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
- TempMovieReadRepository (fallback read repository using SQL Server)

### Database Contexts

1. **WriteDbContext**
   - Entity Framework Core context for SQL Server
   - Handles write operations

2. **Marten Document Store**
   - PostgreSQL-based document store and event store
   - Handles event sourcing with resilient error handling
   - Configured with command timeout and auto-create schema options

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

## Resilient Design

The application implements resilient error handling for database connections:

1. **Graceful Degradation**
   - Falls back to SQL Server for read operations when PostgreSQL is unavailable
   - TempMovieReadRepository provides SQL Server-based implementation of IMovieReadRepository
   - Application continues to function with full capabilities even when PostgreSQL is unavailable

2. **Enhanced Error Handling**
   - Comprehensive error handling in the EventStoreService with retry logic
   - Specific exception handling for PostgreSQL errors
   - Prevents application crashes when PostgreSQL connection fails
   - Detailed error logging for troubleshooting
   - Graceful degradation to continue application execution without event store functionality

3. **Retry Mechanism**
   - Configurable maximum retry attempts (default: 5)
   - Configurable delay between retries (default: 5 seconds)
   - Intelligent backoff strategy for connection attempts
   - Clear logging of retry attempts for monitoring

4. **Database Configuration**
   - Command timeout of 30 seconds to prevent long waits during connection attempts
   - Auto-create schema objects set to ensure database schema is created when available
   - Optimized artifact workflow and lightweight sessions for better performance
   - Resilient connection string configuration

5. **Testing Tools**
   - TestPostgresConnection utility for diagnosing PostgreSQL connection issues
   - Multiple connection string formats for troubleshooting

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
   - API: http://localhost:5251/api
   - Swagger: http://localhost:5251/swagger

## Troubleshooting

### PostgreSQL Connection Issues

If encountering PostgreSQL connection issues:

1. Verify Docker container is running:
   ```
   docker ps | findstr postgres
   ```

2. Check PostgreSQL logs:
   ```
   docker logs movie-catalog-postgres
   ```

3. Test connection using the TestPostgresConnection utility:
   ```
   dotnet run --project TestPostgresConnection/TestPostgresConnection.csproj
   ```

4. Verify connection string in appsettings.json matches Docker container configuration

The application will continue to function using SQL Server for read operations even if PostgreSQL is unavailable.
