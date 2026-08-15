BookStore

A production-oriented .NET 10 microservices-based BookStore project focused on practical software architecture, Domain-Driven Design, messaging, and distributed-systems patterns.

Architecture

BookStore
├── src
│   ├── BuildingBlocks
│   │   └── BookStore.BuildingBlocks
│   └── Services
│       ├── IdentityService
│       ├── ProductService
│       └── NotificationService
└── tests

Services

IdentityService — authentication, authorization, users, roles, permissions, refresh tokens, and JWT.

ProductService — product management and product domain logic.

NotificationService — consumes integration events and processes notifications.

Architectural Principles

The project demonstrates:

Clean Architecture

Domain-Driven Design (DDD)

Domain Events

Integration Events

CQRS-oriented application structure

Dependency Inversion

Repository Pattern

Unit of Work

Transactional Outbox Pattern

Event-driven communication

RabbitMQ messaging

Redis / distributed caching

Messaging

Inter-service communication uses asynchronous integration events.

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

The Transactional Outbox Pattern helps keep database changes and event persistence in the same transaction, reducing the risk of losing events between database updates and message publishing.

BuildingBlocks

Shared cross-cutting concerns are gradually extracted into:

src/BuildingBlocks/BookStore.BuildingBlocks

Current shared concerns include:

Domain abstractions

Aggregate root contracts

Domain event contracts

Integration event contracts

Event bus abstractions

RabbitMQ infrastructure

Transactional Outbox infrastructure

Outbox message processing

Authorization building blocks

The shared layer is intentionally kept independent of service-specific domain models.

Persistence

The services use:

Entity Framework Core 10

PostgreSQL

EF Core migrations

Transactional Outbox

Service-specific EF Core configurations are kept inside each service's Infrastructure layer.

Technologies

.NET 10

C#

ASP.NET Core

Entity Framework Core

PostgreSQL

RabbitMQ

Redis

MediatR

FluentValidation

Docker / Docker Compose

xUnit

Moq

Development

git clone https://github.com/MHAlmaspoor/BookStore.git
cd BookStore

dotnet restore
dotnet build

Run infrastructure dependencies with Docker Compose when applicable:

docker compose up -d

Configuration

Environment-specific configuration and secrets should not be committed to the repository.

Use local .env files or development configuration for credentials and environment-specific settings.

An .env.example file can document required variables without exposing real credentials.

Testing

Run all tests with:

dotnet test

Git Workflow

Development is organized around feature branches:

main
  ↑
develop
  ↑
feature/*

Features are developed and committed independently before being integrated into develop.

## Current Status

### Completed

- [x] Product Domain
- [x] Identity Domain
- [x] Domain Events
- [x] Integration Events
- [x] RabbitMQ Event Bus
- [x] Routing Keys
- [x] Notification Consumer
- [x] Outbox Pattern
- [x] Shared Outbox Processor

### Next

- [ ] Redis
- [ ] Caching
- [ ] Distributed caching
- [ ] ...



This project is intentionally evolving beyond a simple CRUD application to explore real-world microservices architecture and engineering practices.

License

MIT
