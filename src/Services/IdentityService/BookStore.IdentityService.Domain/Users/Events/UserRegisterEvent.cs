
using BookStore.IdentityService.Domain.Events;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Domain.Users.Events;

public sealed record UserRegisteredEvent(UserId UserId,string Email) : DomainEvent;
