using FluentValidation;

namespace Application.Users.Update.Ban;

internal sealed class UnbanUserCommandValidator : AbstractValidator<UnbanUserCommand>
{
    public UnbanUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}