using FluentValidation;

namespace Application.Orders.Delete.OrderItem;

internal sealed class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
{
    public DeleteOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.OrderItemId)
            .NotEmpty();
    }
}