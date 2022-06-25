namespace Mpp.Architecture.Core.Test.Domain;

using FluentValidation;
using MediatR;
using Mpp.Architecture.Core.Domain.DomainBusinessHanlder;
using Mpp.Architecture.Core.Domain.Services;

public class CreateOrderHandler
{
    public class Request : IRequest<IDomainResult<Order>>
    {
        public DateTime OrderDate { get; set; }
    }

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
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

    public class Handler : CreateHandlerCore<Request, Order>
    {
        public Handler(IOrderRepository orderRepository,
            IDomainValidationService domainValidationService,
            IDomainMapper domainMapper)
            : base(domainMapper, orderRepository, domainValidationService)
        {

        }
    }
}