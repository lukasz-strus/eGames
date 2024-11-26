using FluentValidation;

namespace Application.Libraries.Delete;

internal sealed class DeleteLibraryGameCommandValidator : AbstractValidator<DeleteLibraryGameCommand>
{
    public DeleteLibraryGameCommandValidator()
    {
        RuleFor(x => x.LibraryGameId)
            .NotEmpty();
    }
}