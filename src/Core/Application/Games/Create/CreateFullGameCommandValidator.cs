using FluentValidation;

namespace Application.Games.Create;

internal class CreateFullGameCommandValidator : AbstractValidator<CreateFullGameCommand>
{
    public CreateFullGameCommandValidator()
    {
        RuleFor(x => x.Game.Name)
            .NotEmpty()
            .MaximumLength(1);

        RuleFor(x => x.Game.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Game.Price)
            .NotEmpty()
            .GreaterThan(0);
    }
}