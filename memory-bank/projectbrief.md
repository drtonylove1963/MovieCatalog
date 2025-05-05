# MovieCatalog Project Brief

## Project Overview
MovieCatalog is a comprehensive movie management system built using a modern .NET Clean Architecture approach. The application allows users to catalog, search, and manage information about movies, actors, and genres.

## Core Requirements
1. Maintain a catalog of movies with detailed information (title, description, release year, duration, rating)
2. Track actors associated with each movie
3. Categorize movies by genres
4. Support CRUD operations for movies, actors, and genres
5. Provide search and filtering capabilities
6. Implement event-driven architecture for system extensibility

## Technical Goals
1. Implement Clean Architecture with distinct layers (Domain, Application, Infrastructure, API)
2. Use Domain-Driven Design (DDD) principles with aggregates and domain events
3. Implement CQRS pattern with separate read and write models
4. Use event sourcing for data persistence and auditing
5. Provide a RESTful API for client applications

## Architecture Decisions
- **Domain Layer**: Contains core business entities, aggregates, domain events, and repository interfaces
- **Application Layer**: Contains business logic, commands, queries, and behaviors using MediatR
- **Infrastructure Layer**: Implements persistence, messaging, and external services
- **API Layer**: Provides RESTful endpoints for client applications

## Data Storage
- Write Model: SQL Server for transactional data
- Read Model: PostgreSQL with Marten for document storage and event sourcing
- Messaging: RabbitMQ for event distribution

## Future Considerations
- User authentication and authorization
- Rating and review system
- Integration with external movie APIs
- Advanced search and recommendation features
