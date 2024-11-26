using FluentValidation;

namespace Application.Games.Update.MakePublic;

internal sealed class UnpublishGameCommandValidator : AbstractValidator<UnpublishGameCommand>
{
    public UnpublishGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}