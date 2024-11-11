using Domain.Users;
using FluentValidation;

namespace Application.Users.Create.User;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private const string UserWithThisUserNameAlreadyExists = "User with this username already exists.";

    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.User.UserName)
            .NotEmpty()
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var user = await userRepository.GetByUserName(value, cancellationToken);

                if (user is not null)
                    context.AddFailure(UserWithThisUserNameAlreadyExists);
            });
    }
}