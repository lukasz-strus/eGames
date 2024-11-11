using FluentValidation;

namespace Application.Games.Delete;

internal sealed class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
{
    public DeleteGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}