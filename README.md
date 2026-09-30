````markdown
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
├── tests
├── api-tests
├── docker-compose.yml
├── Dockerfile
├── Dockerfile.identity
└── Dockerfile.notification
````

## Services

### IdentityService

Handles:

* Authentication
* Authorization
* Users
* Roles
* Permissions
* Refresh tokens
* JWT-based security

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
* Resilience policies

### NotificationService

Consumes integration events asynchronously through RabbitMQ.

Current consumers include:

* `product.created`
* `user.registered`

The service runs as a .NET Worker and processes RabbitMQ messages asynchronously.

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
* Distributed tracing
* Resilience patterns
* Containerized microservices

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
NotificationService Consumer
```

The Transactional Outbox Pattern keeps database changes and event persistence within the same transaction, reducing the risk of losing events between database updates and message publishing.

## Event Flow

A typical user registration flow is:

```text
Client
  ↓
IdentityService
  ↓
PostgreSQL
  ↓
Transactional Outbox
  ↓
Outbox Processor
  ↓
RabbitMQ
  ↓
NotificationService
  ↓
UserRegisteredHandler
```

The same event-driven architecture is used for ProductService integration events.

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
* Redis timeout handling
* Retry strategies
* Circuit breaker protection
* Graceful fallback behavior

## Docker

All three microservices are containerized:

```text
┌─────────────────────────────────────────────┐
│              Docker Compose                 │
│                                             │
│  ┌───────────────┐   ┌──────────────────┐  │
│  │ ProductService│   │ IdentityService  │  │
│  │    :5039      │   │      :5040       │  │
│  └───────┬───────┘   └────────┬─────────┘  │
│          │                    │             │
│          └──────────┬─────────┘             │
│                     ▼                       │
│              ┌─────────────┐                │
│              │  RabbitMQ   │                │
│              │    :5672    │                │
│              └──────┬──────┘                │
│                     │                       │
│                     ▼                       │
│            ┌─────────────────┐              │
│            │NotificationService│             │
│            │    Worker       │              │
│            └─────────────────┘              │
│                                             │
│  PostgreSQL :5432                           │
│  Redis      :6379                           │
│  Jaeger     :16686 / OTLP 4317             │
└─────────────────────────────────────────────┘
```

Dockerfiles:

```text
Dockerfile
Dockerfile.identity
Dockerfile.notification
```

The Docker images use multi-stage builds to keep runtime images smaller and separate build-time dependencies from production runtime dependencies.

## Docker Compose Infrastructure

The local distributed environment includes:

* PostgreSQL
* pgAdmin
* RabbitMQ
* Redis
* Jaeger
* ProductService
* IdentityService
* NotificationService

Start the complete environment with:

```bash
docker compose up -d
```

Check running containers:

```bash
docker compose ps
```

View service logs:

```bash
docker logs bookstore-productservice
docker logs bookstore-identityservice
docker logs bookstore-notificationservice
```

## Observability

The BookStore microservices provide distributed observability using OpenTelemetry.

### Distributed Tracing

Tracing is implemented across:

* ASP.NET Core HTTP requests
* EF Core database operations
* PostgreSQL
* Redis cache operations
* Transactional Outbox processing
* RabbitMQ event publishing
* RabbitMQ event consumption
* Cross-service trace context propagation

A distributed trace can follow a flow such as:

```text
HTTP Request
     ↓
ProductService / IdentityService
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

Trace context is persisted in the Outbox message and propagated through RabbitMQ using the `traceparent` header.

### Jaeger

Distributed traces can be visualized using Jaeger.

Jaeger UI:

```text
http://localhost:16686
```

The tracing infrastructure uses OpenTelemetry and OTLP to export telemetry data to Jaeger.

## Health Checks

Health checks cover infrastructure dependencies such as:

* PostgreSQL
* Redis
* RabbitMQ

The readiness endpoint currently used for validation is:

```text
/api/health/ready
```

For example:

```text
GET /api/health/ready
```

The endpoint reports the readiness state of the service dependencies.

## Resilience

The infrastructure includes resilience mechanisms for distributed dependencies such as:

* Redis timeout handling
* Redis fallback behavior
* Circuit breaker protection
* Retry strategies
* Transactional Outbox retry handling
* Exponential backoff

## CI

GitHub Actions currently performs:

```text
Checkout
   ↓
.NET 10 Setup
   ↓
Restore
   ↓
Build
   ↓
Test
```

The CI pipeline is being extended to validate Docker image builds for all three microservices:

```text
Restore
   ↓
Build
   ↓
Test
   ↓
Docker Build
   ├── ProductService
   ├── IdentityService
   └── NotificationService
```

Docker images are currently built for validation only. Publishing images to a container registry will be introduced as part of the CD phase.

## Technologies

* .NET 10
* C#
* ASP.NET Core
* .NET Worker Services
* Entity Framework Core 10
* PostgreSQL
* RabbitMQ
* Redis
* MediatR
* FluentValidation
* OpenTelemetry
* Jaeger
* Docker
* Docker Compose
* GitHub Actions
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

Run the complete local environment:

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

Features are developed and committed independently before being integrated into `develop`.

The project follows Conventional Commit style for commit messages.

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
* [x] Distributed Observability
* [x] Jaeger Integration
* [x] Performance and Load Testing
* [x] ProductService Dockerization
* [x] IdentityService Dockerization
* [x] NotificationService Dockerization
* [x] Docker Compose Environment
* [x] CI Restore / Build / Test

### In Progress

* [ ] Docker image builds in CI
* [ ] CI/CD container image pipeline

### Next

* [ ] Container registry integration
* [ ] CD pipeline
* [ ] Further Product APIs
* [ ] Further Microservices
* [ ] Production hardening
* [ ] Security hardening

## Project Direction

This project is intentionally evolving beyond a simple CRUD application.

The goal is to incrementally build a production-oriented microservices system while applying practical architectural patterns, distributed messaging, transactional consistency, caching, resilience, observability, containerization, and CI/CD practices.

## License

MIT

```
```
