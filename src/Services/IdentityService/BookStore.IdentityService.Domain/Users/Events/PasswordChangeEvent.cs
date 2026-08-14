using System.Text.Json.Serialization;
using BookStore.IdentityService.Domain.Events;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Domain.Users.Events;
public sealed record PasswordChangedEvent(UserId UserId) : DomainEvent;

