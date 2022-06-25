namespace Mpp.Architecture.Core.Domain.DomainBusinessHanlder;

using MediatR;
using Mpp.Architecture.Core.Domain.Common;
using Mpp.Architecture.Core.Domain.Entities;
using Mpp.Architecture.Core.Domain.Persistence;
using Mpp.Architecture.Core.Domain.Services;

public abstract class UpdateHandlerCore<TRequest, TEntity> : IRequestHandler<TRequest, IDomainResult<TEntity>>
    where TEntity : Entity, new()
    where TRequest : IRequest<IDomainResult<TEntity>>
{
    protected readonly IDomainMapper Mapper;
    protected readonly IRepository<TEntity> Repository;
    protected readonly IDomainValidationService DomainValidationService;

    protected UpdateHandlerCore(IDomainMapper mapper,
        IRepository<TEntity> repository,
        IDomainValidationService domainValidationService)
    {
        Mapper = mapper;
        Repository = repository;
        DomainValidationService = domainValidationService;
    }

    public async Task<IDomainResult<TEntity>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var updatingEntity = await GetEntity(request, cancellationToken);
        if (updatingEntity == null)
        {
            throw new DomainException(DomainProblemContainer.EntityNotFound, typeof(TEntity).Name);
        }

        updatingEntity = await UpdateEntity(request, updatingEntity, cancellationToken);
        Validate(updatingEntity);
        Repository.Update(updatingEntity);
        return new DomainResult<TEntity>(updatingEntity);
    }

    protected abstract Task<TEntity?> GetEntity(TRequest request, CancellationToken cancellationToken);

    protected virtual async Task<TEntity> UpdateEntity(TRequest request, TEntity updatingEntity, CancellationToken cancellationToken)
    {
        return await Task.FromResult(Mapper.Map<TRequest, TEntity>(request));
    }

    protected virtual void Validate(TEntity entity)
    {
        var validationresult = DomainValidationService.Validate(entity);
        if (!validationresult.IsValid)
        {
            throw new DomainException(DomainProblemContainer.BadEntity, typeof(TEntity).Name, validationresult.ToString());
        }
    }
}
