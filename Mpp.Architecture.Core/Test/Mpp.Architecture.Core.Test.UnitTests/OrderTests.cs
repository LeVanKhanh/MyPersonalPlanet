namespace Mpp.Architecture.Core.Test.UnitTests
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
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
    }
}