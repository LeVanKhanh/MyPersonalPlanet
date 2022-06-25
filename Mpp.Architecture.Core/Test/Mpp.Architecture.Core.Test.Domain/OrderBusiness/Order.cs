namespace Mpp.Architecture.Core.Test.Domain
{
    using Mpp.Architecture.Core.Domain.Entities;

    public class Order : Entity<int>
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(int orderId, DateTime orderDate) : this(orderDate)
        {
            OrderId = orderId;
        }

        public Order(DateTime orderDate) : this()
        {
            OrderDate = orderDate;
        }
    }
}
