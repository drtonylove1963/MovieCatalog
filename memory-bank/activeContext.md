# MovieCatalog Active Context

## Current Focus
- Initial project setup and architecture implementation
- Core domain model development (Movie, Actor, Genre aggregates)
- Infrastructure configuration for CQRS and Event Sourcing
- Pipeline behaviors for validation, logging, and exception handling

## Recent Changes
- Established Clean Architecture structure with four layers
- Implemented domain aggregates with event sourcing
- Configured dual database approach (SQL Server for writes, PostgreSQL for reads)
- Set up MediatR pipeline behaviors for cross-cutting concerns
- Integrated Wolverine for message handling with RabbitMQ

## Active Decisions
- Using aggregate roots for Movie, Actor, and Genre entities
- Implementing CQRS with separate read and write models
- Using event sourcing for data persistence and auditing
- Leveraging Marten for document storage and event sourcing
- Using MediatR pipeline behaviors for cross-cutting concerns

## Current Considerations
- Optimizing read model projections for performance
- Ensuring consistency between read and write models
- Implementing proper error handling and validation
- Designing effective API endpoints for client applications
- Setting up proper logging and monitoring

## Immediate Next Steps
1. Complete the implementation of command and query handlers
2. Implement remaining API controllers for all aggregates
3. Add comprehensive validation rules for all commands
4. Set up proper error handling and response formatting
5. Implement unit and integration tests for core functionality
