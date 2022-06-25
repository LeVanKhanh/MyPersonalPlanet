namespace Mpp.Architecture.Core.Infrastructure.Persistance
{
    using Microsoft.EntityFrameworkCore;
    using Mpp.Architecture.Core.Domain.Entities;
    using Mpp.Architecture.Core.Domain.Persistence;

    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected BaseRepository(BaseDBContext baseDBContext)
        {
            _dbSet = baseDBContext.Set<TEntity>();
            UnitOfWork = baseDBContext;
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public virtual TEntity Remove(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            return _dbSet.Update(entity).Entity;
        }

        public TEntity? Find(params object?[]? keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual Task<TEntity> GetAsync(Func<TEntity, bool> predicate)
        {
            return null;
        }

        public virtual Task<TEntity> GetNoTrackingAsync(Func<TEntity, bool> predicate)
        {
            return null;
        }

        public virtual Task<IEnumerable<TEntity>> GetListAsync(Func<TEntity, bool> predicate)
        {
            return null;
        }

        public Task<IEnumerable<TEntity>> GetListNotrackingAsync(Func<TEntity, bool> predicate)
        {
            return null;
        }

        public IUnitOfWork UnitOfWork { get; private set; }
    }
}
