namespace Mpp.Architecture.Core.Infrastructure.Persistance;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Mpp.Architecture.Core.Domain.Entities;
using Mpp.Architecture.Core.Domain.Persistence;
using System.Threading;
using System.Threading.Tasks;

public abstract class BaseDBContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    protected BaseDBContext(DbContextOptions options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(_mediator);
        return await SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(IMediator mediator)
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
