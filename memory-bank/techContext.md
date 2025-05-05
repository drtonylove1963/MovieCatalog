# MovieCatalog Technical Context

## Technology Stack
- **.NET 8.0**: Core framework for application development
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for SQL Server data access
- **Marten**: Document DB and Event Store for PostgreSQL
- **MediatR**: Mediator implementation for CQRS
- **FluentValidation**: Validation library
- **Wolverine**: Command and event handling
- **RabbitMQ**: Message broker for event distribution
- **Swagger/OpenAPI**: API documentation

## Data Storage
- **SQL Server**: Primary write database for transactional data
- **PostgreSQL**: Document database and event store using Marten
- **Event Store**: Persistent storage for domain events

## Development Tools
- **Visual Studio**: Primary IDE
- **Docker**: Containerization for development and deployment
- **Docker Compose**: Multi-container Docker applications

## Project Structure
```
MovieCatalog/
├── MovieCatalog.Api/             # API Layer
│   ├── Controllers/              # API Controllers
│   └── Program.cs                # Application entry point
├── MovieCatalog.Application/     # Application Layer
│   ├── Common/                   # Shared components
│   │   ├── Behaviors/            # MediatR pipeline behaviors
│   │   └── Interfaces/           # Application interfaces
│   └── DependencyInjection.cs    # DI configuration
├── MovieCatalog.Domain/          # Domain Layer
│   ├── Aggregates/               # Domain aggregates
│   │   ├── ActorAggregate/       # Actor aggregate
│   │   ├── GenreAggregate/       # Genre aggregate
│   │   └── MovieAggregate/       # Movie aggregate
│   ├── Common/                   # Shared domain components
│   └── Repositories/             # Repository interfaces
└── MovieCatalog.Infrastructure/  # Infrastructure Layer
    ├── EventHandlers/            # Domain event handlers
    ├── Persistence/              # Data persistence
    ├── Repositories/             # Repository implementations
    └── Services/                 # Infrastructure services
```

## Key Dependencies
- **MediatR**: Command and query handling
- **FluentValidation**: Request validation
- **Marten**: Document DB and event sourcing
- **Wolverine**: Message handling and routing
- **Entity Framework Core**: ORM for SQL Server
- **Newtonsoft.Json**: JSON serialization/deserialization
- **Swashbuckle**: Swagger/OpenAPI documentation

## Configuration
- **appsettings.json**: Application configuration
- **appsettings.Development.json**: Development-specific configuration
- **Connection Strings**:
  - SQL Server (Write DB)
  - PostgreSQL (Read DB and Event Store)
  - RabbitMQ (Message Broker)

## Deployment Considerations
- **Docker Containers**: Application components containerized
- **Docker Compose**: Local development environment
- **Database Migrations**: EF Core migrations for SQL Server
- **Event Store Setup**: Marten configuration for PostgreSQL
- **Message Broker Configuration**: RabbitMQ exchanges and queues
