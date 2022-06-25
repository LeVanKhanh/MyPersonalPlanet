namespace Mpp.Architecture.Core.Test.Domain
{
    using Mpp.Architecture.Core.Domain.Entities;

    public class OrderItem : Entity<int>
    {
        public OrderItem(int orderId,
                         int productId,
                         decimal price,
                         int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        public int OrderId { get; private set; }
        public decimal Price { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public Order Order { get; set; }
        public void UpdateQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}
