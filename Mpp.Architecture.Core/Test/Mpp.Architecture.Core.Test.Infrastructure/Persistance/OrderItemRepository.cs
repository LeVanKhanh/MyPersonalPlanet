namespace Mpp.Architecture.Core.Test.Infrastructure.Persistance
{
    using Mpp.Architecture.Core.Infrastructure.Persistance;
    using Mpp.Architecture.Core.Test.Domain;

    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(TestDbContext testDbContext) : base(testDbContext)
        {

        }

        public OrderItem? GetItem(int orderId, int productId)
        {
            return _dbSet.Where(w => w.OrderId == orderId && w.ProductId == productId)
                    .FirstOrDefault();    
        }
    }
}
