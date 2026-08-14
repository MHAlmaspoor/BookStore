using System.Text.Json.Serialization;
using BookStore.IdentityService.Domain.Events;
using BookStore.IdentityService.Domain.ValueObjects;


public sealed record UserDeactivatedEvent(UserId UserId) : DomainEvent;

