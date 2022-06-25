namespace Mpp.Architecture.Core.Test.Domain;

using FluentValidation;
using MediatR;
using Mpp.Architecture.Core.Domain.DomainBusinessHanlder;
using Mpp.Architecture.Core.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

public sealed class UpdateOrderHandler
{
    public class Request : IRequest<IDomainResult<Order>>
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(r => r.OrderDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }

    public class OrderCreationMapper : IDomainMapper<Request, Order>
    {
        public Order Map(Request source, Order destination)
        {
            destination.OrderDate = source.OrderDate;
            return destination;
        }
    }

    public class Handler : UpdateHandlerCore<Request, Order>
    {
        public Handler(IOrderRepository orderRepository,
            IDomainValidationService domainValidationService,
            IDomainMapper domainMapper)
            : base(domainMapper, orderRepository, domainValidationService)
        {

        }

        protected override async Task<Order?> GetEntity(Request request, CancellationToken cancellationToken)
        {
            var order = Repository.Find(request.OrderId);
            return await Task.FromResult(order);
        }
    }
}