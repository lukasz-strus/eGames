using FluentValidation;

namespace Application.Users.Create.User;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User.UserName)
            .NotEmpty();
    }
}