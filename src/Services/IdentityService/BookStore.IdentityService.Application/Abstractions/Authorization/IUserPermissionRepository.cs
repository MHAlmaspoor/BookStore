using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Application.Abstractions.Authorization;

public interface IUserPermissionRepository
{
    Task<IReadOnlyCollection<string>> GetPermissionsAsync(UserId userId, CancellationToken cancellationToken = default);
}
