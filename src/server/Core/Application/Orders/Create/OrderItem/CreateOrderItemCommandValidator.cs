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

        RuleFor(x => x.OrderItem.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.OrderItem.CurrencyId)
            .NotEmpty()
            .Must(x => new[] { 1, 2, 3 }.Contains(x))
            .WithMessage("Currency ID must be 1, 2 or 3.");
    }
}