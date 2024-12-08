using Domain.Games;
using FluentValidation;

namespace Application.Games.Create.FullGame;

internal sealed class CreateFullGameCommandValidator : AbstractValidator<CreateFullGameCommand>
{
    private const string GameWithThisNameAlreadyExists = "Game with this name already exists.";

    public CreateFullGameCommandValidator(IGameRepository gameRepository)
    {
        RuleFor(x => x.Game.Name)
            .NotEmpty()
            .MaximumLength(100)
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var game = await gameRepository.GetByName(value, cancellationToken);

                if (game is not null)
                    context.AddFailure(GameWithThisNameAlreadyExists);
            });

        RuleFor(x => x.Game.Description)
            .NotEmpty();

        RuleFor(x => x.Game.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Game.CurrencyId)
            .NotEmpty()
            .Must(x => new[] { 1, 2, 3 }.Contains(x))
            .WithMessage("Currency ID must be 1, 2 or 3.");

        RuleFor(x => x.Game.ReleaseDate)
            .NotEmpty();

        RuleFor(x => x.Game.Publisher)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Game.DownloadLink)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Game.FileSize)
            .NotEmpty();

        RuleFor(x => x.Game.ImageUrl)
            .NotEmpty();
    }
}