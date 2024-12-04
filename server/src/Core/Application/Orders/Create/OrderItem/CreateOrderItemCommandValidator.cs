using FluentValidation;

namespace Application.Orders.Create.OrderItem;

internal sealed class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
{
    public CreateOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.OrderItem.GameId)
            .NotEmpty();
    }
}