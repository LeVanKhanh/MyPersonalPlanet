namespace Mpp.Architecture.Core.Test.Infrastructure.Persistance
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Mpp.Architecture.Core.Infrastructure.Persistance;
    using Mpp.Architecture.Core.Test.Domain;

    public class TestDbContext : BaseDBContext
    {
        public TestDbContext(DbContextOptions options,
            IMediator mediator) : base(options, mediator)
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
    }
}
