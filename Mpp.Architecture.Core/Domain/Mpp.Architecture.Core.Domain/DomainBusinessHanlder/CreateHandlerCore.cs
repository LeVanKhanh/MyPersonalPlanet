namespace Mpp.Architecture.Core.Domain.DomainBusinessHanlder;

using MediatR;
using Mpp.Architecture.Core.Domain.Common;
using Mpp.Architecture.Core.Domain.Entities;
using Mpp.Architecture.Core.Domain.Persistence;
using Mpp.Architecture.Core.Domain.Services;

public abstract class CreateHandlerCore<TRequest, TEntity> : IRequestHandler<TRequest, IDomainResult<TEntity>>
        where TEntity : Entity
        where TRequest : IRequest<IDomainResult<TEntity>>
{
    protected readonly IDomainMapper Mapper;
    protected readonly IRepository<TEntity> Repository;
    protected readonly IDomainValidationService DomainValidationService;

    protected CreateHandlerCore(IDomainMapper mapper,
        IRepository<TEntity> repository,
        IDomainValidationService domainValidationService)
    {
        Mapper = mapper;
        Repository = repository;
        DomainValidationService = domainValidationService;
    }

    public async Task<IDomainResult<TEntity>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var creatingEntity = await CreateEntity(request);
        Validate(creatingEntity);
        Repository.Add(creatingEntity);
        return new DomainResult<TEntity>(creatingEntity);
    }

    protected virtual async Task<TEntity> CreateEntity(TRequest request)
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