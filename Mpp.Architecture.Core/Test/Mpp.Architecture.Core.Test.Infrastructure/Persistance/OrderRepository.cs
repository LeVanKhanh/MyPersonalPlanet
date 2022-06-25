namespace Mpp.Architecture.Core.Test.Infrastructure.Persistance
{
    using Mpp.Architecture.Core.Infrastructure.Persistance;
    using Mpp.Architecture.Core.Test.Domain;

    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(TestDbContext testDbContext) : base(testDbContext)
        {

        }
    }
}
