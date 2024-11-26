using FluentValidation;

namespace Application.Games.Update.Restore;

internal sealed class RestoreGameCommandValidator : AbstractValidator<RestoreGameCommand>
{
    public RestoreGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}