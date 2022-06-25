namespace Mpp.Architecture.Core.Test.Domain
{
    using FluentValidation;

    internal class OrderValidator: AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.OrderDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
