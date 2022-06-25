namespace Mpp.Architecture.Core.Test.Domain;
using MediatR;
using Mpp.Architecture.Core.Domain.DomainBusinessHanlder;
using System.Threading;
using System.Threading.Tasks;

public sealed class DeleteOrderHandler
{
    public class Request : IRequest<IDomainResult<Order>>
    {
        public int OrderId { get; set; }
    }

    public class Handler : DeleteHandlerCore<Request, Order>
    {
        public Handler(IOrderRepository orderRepository) : base(orderRepository)
        {

        }

        protected override async Task<Order?> GetEntity(Request request, CancellationToken cancellationToken)
        {
            var order = Repository.Find(request.OrderId);
            return await Task.FromResult(order);
        }
    }
}