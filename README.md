# Movie Catalog API

A comprehensive API for managing a movie catalog with actors, genres, and ratings.

## Project Overview

This project is a Movie Catalog application that uses a combination of SQL Server and PostgreSQL databases, along with RabbitMQ for messaging. The application is structured using a CQRS pattern and utilizes ASP.NET Core for the web API.

## Features

- **Movies Management**: Create, retrieve, update, and delete movies
- **Actors Management**: Create, retrieve, update, and delete actors
- **Genres Management**: Create, retrieve, update, and delete genres
- **Ratings**: Add and update movie ratings
- **Search**: Search for movies by title or genre
- **Pagination**: Paginated results for better performance
- **Event Sourcing**: Track all changes to entities using event sourcing with Marten

## Architecture

- **CQRS Pattern**: Command Query Responsibility Segregation for better separation of concerns
- **Event Sourcing**: Using Marten for event sourcing with resilient error handling
- **Message Queue**: RabbitMQ for asynchronous processing (temporarily disabled)
- **Multiple Databases**: 
  - SQL Server for write operations and temporary read operations
  - PostgreSQL for event store and future read operations

## Tech Stack

- ASP.NET Core 8.0
- Entity Framework Core
- Marten (for event sourcing)
- SQL Server (Docker)
- PostgreSQL (Docker)
- RabbitMQ (Docker)
- Swagger for API documentation

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker Desktop
- Git

### Setup

1. Clone the repository:
   ```
   git clone <repository-url>
   cd MovieCatalog
   ```

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

### Database Access

#### PostgreSQL

- **Connection Details**:
  - Host: `localhost` (from host machine) or `movie-catalog-postgres` (from Docker network)
  - Port: `5432`
  - Database: `movie_catalog`
  - Username: `postgres`
  - Password: `Password123!`

- **PgAdmin Access**:
  - URL: http://localhost:5050
  - Login: `admin@moviedb.com`
  - Password: `Password123!`
  - To connect to PostgreSQL in pgAdmin, use the server name `movie-catalog-postgres`

#### SQL Server

- **Connection Details**:
  - Host: `127.0.0.1`
  - Port: `1433`
  - Database: `MovieCatalog`
  - Username: `sa`
  - Password: `Password123!`

## Sample Data

The application comes with pre-seeded sample data including:

- **Movies**: The Shawshank Redemption, The Godfather, Inception, Forrest Gump, The Dark Knight
- **Actors**: Tom Hanks, Meryl Streep, Leonardo DiCaprio, and more
- **Genres**: Action, Comedy, Drama, Science Fiction, Horror, Romance, Thriller

## Project Structure

- **MovieCatalog.Api**: API controllers and DTOs
- **MovieCatalog.Application**: Application services, commands, and queries
- **MovieCatalog.Domain**: Domain entities and business logic
- **MovieCatalog.Infrastructure**: Infrastructure concerns (databases, messaging, etc.)

## Resilient Design

The application implements resilient error handling for database connections:

- **Graceful Degradation**: Falls back to SQL Server for read operations when PostgreSQL is unavailable
- **Enhanced Error Handling**: Comprehensive error handling in the EventStoreService with retry logic and specific PostgreSQL exception handling
- **Retry Mechanism**: Configurable retry attempts and delay between retries for PostgreSQL connections
- **Command Timeout**: Configurable command timeout to prevent long waits during connection attempts
- **Auto-Create Schema**: Automatic schema creation for PostgreSQL when the database becomes available
- **Continuous Operation**: Application continues to function with full capabilities even when PostgreSQL is unavailable

### PostgreSQL Schema Structure

The event sourcing system uses the following PostgreSQL schema structure:

- **Schema**: `movie_catalog`
- **Tables**:
  - `mt_events`: Stores all domain events with their data in JSONB format
  - `mt_streams`: Tracks event streams for aggregates and their versions
  - `mt_doc_movie_read_model`: Stores the projected read models for movies

This schema is automatically created when the application starts, or can be manually created if needed.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
