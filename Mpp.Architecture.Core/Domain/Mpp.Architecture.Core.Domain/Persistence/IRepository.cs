namespace Mpp.Architecture.Core.Domain.Persistence
{
    using Mpp.Architecture.Core.Domain.Entities;

    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity Add(TEntity entity);
        TEntity Remove(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity? Find(params object?[]? keyValues);
        IUnitOfWork UnitOfWork { get; }
    }
}
