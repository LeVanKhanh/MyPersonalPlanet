namespace Mpp.Architecture.Core.Domain.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
