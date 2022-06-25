namespace Mpp.Architecture.Core.Test.Domain.OrderItemBusiness
{
    using MediatR;
    using Mpp.Architecture.Core.Test.Domain;

    public class AddOrderItemHandler
    {
        public class Request : IRequest<Response>
        {
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public class Response
        {
            public OrderItem? OrderItem { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IOrderItemRepository _orderRepository;
            public Handler(IOrderItemRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (IsExist(request.OrderId, request.ProductId, out var orderItem))
                {
                    orderItem?.UpdateQuantity(request.Quantity);
                }
                else
                {
                    orderItem = new OrderItem(request.OrderId, request.ProductId, request.Price, request.Quantity);
                    _orderRepository.Add(orderItem);
                }

                await _orderRepository.UnitOfWork.SaveEntitiesAsync();
                return new Response
                {
                    OrderItem = orderItem
                };
            }

            private bool IsExist(int orderId, int productId, out OrderItem? orderItem)
            {
                orderItem = _orderRepository.GetItem(orderId, productId);
                return orderItem != null;
            }
        }
    }
}
