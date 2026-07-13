namespace BookStore.ProductServicec.Application.Abstraction;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken=default);
}
