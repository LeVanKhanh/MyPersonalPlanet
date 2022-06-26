namespace Mpp.Architecture.Core.Test.UnitTests;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mpp.Architecture.Core.Domain.Persistence;
using Mpp.Architecture.Core.Test.Application.DependencyInjection;
using Mpp.Architecture.Core.Test.Infrastructure.Persistance;
using System;
using System.Threading.Tasks;
using Xunit;

public class OrderTests
{
    private readonly ServiceCollection _services;
    public OrderTests()
    {
        _services = new ServiceCollection();
        _services.AddTestApplication();
        _services.AddMediatR(typeof(ApplicationDedepencyInjection));
        _services.AddDbContext<TestDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
    }

    [Fact]
    public async Task CreateOrder_Success()
    {
        var provider = _services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var result = await mediator.Send(new Domain.CreateOrderHandler.Request
        {
            OrderDate = DateTime.Now
        });
        Assert.True(result.Result?.Id > 0);
    }

    [Fact]
    public async Task UpdateOrder_Success()
    {
        var provider = _services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var createresult = await mediator.Send(new Domain.CreateOrderHandler.Request
        {
            OrderDate = DateTime.Now
        });

        var uof = provider.GetRequiredService<IUnitOfWork>();
        await uof.SaveEntitiesAsync();

        var updatedDate = DateTime.Now.AddMinutes(2);
        var updateResult = await mediator.Send(new Domain.UpdateOrderHandler.Request
        {
            OrderId = createresult.Result.Id,
            OrderDate = updatedDate
        });
        Assert.Equal(updatedDate, updateResult.Result?.OrderDate);
    }

    [Fact]
    public async Task DeleteOrder_Success()
    {
        var provider = _services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var createresult = await mediator.Send(new Domain.CreateOrderHandler.Request
        {
            OrderDate = DateTime.Now
        });

        var uof = provider.GetRequiredService<IUnitOfWork>();
        await uof.SaveEntitiesAsync();

        var deleteResult = await mediator.Send(new Domain.DeleteOrderHandler.Request
        {
            OrderId = createresult.Result.Id,
        });

        Assert.True(deleteResult.Success);
    }
}
