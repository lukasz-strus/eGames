using FluentValidation;

namespace Application.Users.Delete.Role;

internal sealed class RemoveRoleCommandValidator : AbstractValidator<RemoveRoleCommand>
{
    public RemoveRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}