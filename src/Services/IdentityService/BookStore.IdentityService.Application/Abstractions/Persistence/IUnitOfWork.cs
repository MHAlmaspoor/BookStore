namespace BookStore.IdentityService.Application.Abstraction.Persistance;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
}
