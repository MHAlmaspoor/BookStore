# BookStore

A production-oriented .NET 10 microservices-based BookStore project focused on practical software architecture, Domain-Driven Design, messaging, distributed systems, and real-world engineering patterns.

## Architecture

```text
BookStore
├── src
│   ├── BuildingBlocks
│   │   └── BookStore.BuildingBlocks
│   └── Services
│       ├── IdentityService
│       ├── ProductService
│       └── NotificationService
└── tests
```

## Services

### IdentityService

Handles authentication, authorization, users, roles, permissions, refresh tokens, and JWT-based security.

### ProductService

Handles product management and product domain logic.

Current functionality includes:

* Product creation
* Product retrieval
* Product price changes
* Product deletion
* Redis-based distributed caching
* Cache-Aside pattern
* Cache invalidation

### NotificationService

Consumes integration events and processes notifications asynchronously through RabbitMQ.

## Architectural Principles

The project demonstrates:

* Clean Architecture
* Domain-Driven Design (DDD)
* Domain Events
* Integration Events
* CQRS-oriented application structure
* Dependency Inversion
* Repository Pattern
* Unit of Work
* Transactional Outbox Pattern
* Event-driven communication
* RabbitMQ messaging
* Redis distributed caching

The architecture is intentionally evolving as new infrastructure and patterns are introduced.

## Messaging

Inter-service communication uses asynchronous integration events.

```text
Domain Action
     ↓
Domain Event
     ↓
EF Core SaveChanges
     ↓
Transactional Outbox
     ↓
Outbox Processor
     ↓
RabbitMQ
     ↓
Integration Event
     ↓
Consumer
```

The Transactional Outbox Pattern keeps database changes and event persistence within the same transaction, reducing the risk of losing events between database updates and message publishing.

## BuildingBlocks

Shared cross-cutting concerns are gradually extracted into:

```text
src/BuildingBlocks/BookStore.BuildingBlocks
```

Current shared concerns include:

* Domain abstractions
* Aggregate root contracts
* Domain event contracts
* Integration event contracts
* Event bus abstractions
* RabbitMQ infrastructure
* Transactional Outbox infrastructure
* Outbox message processing
* Authorization building blocks

The shared layer is intentionally kept independent of service-specific domain models.

## Persistence

The services use:

* Entity Framework Core 10
* PostgreSQL
* EF Core migrations
* Transactional Outbox

Service-specific EF Core configurations remain inside each service's Infrastructure layer.

## Distributed Caching

ProductService currently uses Redis as a distributed cache.

The caching strategy follows the Cache-Aside pattern:

```text
Get Product
     ↓
   Redis
     │
 ┌───┴────┐
 │        │
HIT      MISS
 │        │
 ▼        ▼
Return  PostgreSQL
          │
          ▼
        Redis
          │
          ▼
        Return
```

For data-changing operations, the corresponding cache entry is invalidated after the database operation succeeds.

### Update

```text
Update Product
      ↓
PostgreSQL
      ↓
Redis.Remove(product:{id})
```

### Delete

```text
Delete Product
      ↓
PostgreSQL
      ↓
Redis.Remove(product:{id})
```

This keeps PostgreSQL as the source of truth while Redis acts as a performance-oriented read cache.

Current caching features include:

* Redis distributed cache
* Cache-Aside pattern
* Cache HIT/MISS handling
* Absolute cache expiration (TTL)
* Cache invalidation on product updates
* Cache invalidation on product deletion

## Messaging and Caching

The project currently explores two important distributed-system concerns independently:

```text
Messaging
    ↓
Transactional Outbox
    ↓
RabbitMQ
```

and:

```text
Caching
    ↓
Redis
    ↓
Cache-Aside
    ↓
Cache Invalidation
```

Future work will address failure scenarios and consistency concerns between distributed components.

## Technologies

* .NET 10
* C#
* ASP.NET Core
* Entity Framework Core 10
* PostgreSQL
* RabbitMQ
* Redis
* MediatR
* FluentValidation
* Docker / Docker Compose
* xUnit
* Moq

## Development

Clone the repository:

```bash
git clone https://github.com/MHAlmaspoor/BookStore.git
cd BookStore
```

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run infrastructure dependencies with Docker Compose when applicable:

```bash
docker compose up -d
```

## Configuration

Environment-specific configuration and secrets should not be committed to the repository.

Use local `.env` files or development configuration for credentials and environment-specific settings.

An `.env.example` file can document required variables without exposing real credentials.

## Testing

Run all tests with:

```bash
dotnet test
```

The project also contains API test collections for manually testing service endpoints and integration flows.

## Git Workflow

Development is organized around feature branches:

```text
main
  ↑
develop
  ↑
feature/*
```

## Observability

The BookStore microservices currently provide end-to-end distributed observability using OpenTelemetry.

### Distributed Tracing

Tracing is implemented across:

- ASP.NET Core HTTP requests
- EF Core database operations
- PostgreSQL
- Redis cache operations
- Transactional Outbox processing
- RabbitMQ event publishing
- RabbitMQ event consumption
- Cross-service trace context propagation

A single distributed trace can follow the complete flow:

```text
HTTP Request
    ↓
ProductService
    ↓
EF Core / PostgreSQL
    ↓
Transactional Outbox
    ↓
RabbitMQ Publish
    ↓
NotificationService
    ↓
RabbitMQ Consumer

```

Trace context is persisted in the Outbox message and propagated through RabbitMQ using the traceparent header.

Jaeger

Distributed traces can be visualized using Jaeger.

Jaeger is available at:

http://localhost:16686

The tracing infrastructure uses OpenTelemetry and OTLP to export telemetry data to Jaeger.

Health Checks

Health checks are available for:

PostgreSQL
Redis
RabbitMQ

Health endpoints:

/api/health
/api/health/live
/api/health/ready
Resilience

The infrastructure includes resilience mechanisms for distributed dependencies such as:

Redis timeout handling
Redis fallback behavior
Circuit breaker protection
Retry strategies
Transactional Outbox retry handling with exponential backoff

Features are developed and committed independently before being integrated into `develop`.

## Current Status

### Completed

* [x] Product Domain
* [x] Identity Domain
* [x] Domain Events
* [x] Integration Events
* [x] RabbitMQ Event Bus
* [x] Routing Keys
* [x] Notification Consumer
* [x] Transactional Outbox
* [x] Shared Outbox Processor
* [x] Redis Infrastructure
* [x] Product Cache-Aside
* [x] Cache Invalidation
* [x] Cache Consistency Strategies
* [x] Retry Strategies for Distributed Operations
* [x] Resilience and Fault Tolerance
* [x] Additional Product APIs
* [x] Observability

### Next

* [ ] Further  Product APIs
* [ ] Further Microservices
* [ ] Performance and Load Testing
* [ ] CI/CD
## Project Direction

This project is intentionally evolving beyond a simple CRUD application.

The goal is to incrementally build a production-oriented microservices system while applying practical architectural patterns, distributed messaging, transactional consistency, caching, resilience, and other real-world software engineering practices.

## License

MIT
