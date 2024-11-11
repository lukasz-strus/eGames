using FluentValidation;

namespace Application.Games.Update.MakePublic;

internal sealed class PublishGameCommandValidator : AbstractValidator<PublishGameCommand>
{
    public PublishGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}