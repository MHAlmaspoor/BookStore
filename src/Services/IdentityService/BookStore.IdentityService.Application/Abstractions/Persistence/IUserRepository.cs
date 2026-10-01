using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync( UserId id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync( Email email, CancellationToken cancellationToken = default);

    Task AddAsync( User user, CancellationToken cancellationToken = default);
}
