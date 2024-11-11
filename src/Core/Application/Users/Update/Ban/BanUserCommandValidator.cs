using FluentValidation;

namespace Application.Users.Update.Ban;

internal sealed class BanUserCommandValidator : AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}