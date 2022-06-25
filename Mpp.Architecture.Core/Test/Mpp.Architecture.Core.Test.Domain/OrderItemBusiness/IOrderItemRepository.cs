namespace Mpp.Architecture.Core.Test.Domain
{
    using Mpp.Architecture.Core.Domain.Persistence;

    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        OrderItem? GetItem(int orderId, int productId);
    }
}
