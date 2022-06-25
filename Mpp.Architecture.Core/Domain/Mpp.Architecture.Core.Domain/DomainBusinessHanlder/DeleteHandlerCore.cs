namespace Mpp.Architecture.Core.Domain.DomainBusinessHanlder;

using MediatR;
using Mpp.Architecture.Core.Domain.Common;
using Mpp.Architecture.Core.Domain.Entities;
using Mpp.Architecture.Core.Domain.Persistence;

public abstract class DeleteHandlerCore<TRequest, TEntity> : IRequestHandler<TRequest, IDomainResult<TEntity>>
    where TEntity : Entity, new()
    where TRequest : IRequest<IDomainResult<TEntity>>
{
    protected readonly IRepository<TEntity> Repository;

    protected DeleteHandlerCore(IRepository<TEntity> repository)
    {
        Repository = repository;
    }

    public async Task<IDomainResult<TEntity>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var deletingEntity = await GetEntity(request, cancellationToken);
        if (deletingEntity == null)
        {
            throw new DomainException(DomainProblemContainer.BadEntity, typeof(TEntity).Name);
        }
        Repository.Remove(deletingEntity);
        return new DomainResult<TEntity>(deletingEntity);
    }

    protected abstract Task<TEntity?> GetEntity(TRequest request, CancellationToken cancellationToken);
}
