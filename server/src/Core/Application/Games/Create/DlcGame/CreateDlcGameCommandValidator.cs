using Domain.Games;
using FluentValidation;

namespace Application.Games.Create.DlcGame;

internal sealed class CreateDlcGameCommandValidator : AbstractValidator<CreateDlcGameCommand>
{
    private const string DlcWithThisNameAlreadyExists = "Dlc with this name already exists.";
    private const string CurrencyIdMustBe1Or2Or3 = "Currency ID must be 1, 2 or 3.";

    public CreateDlcGameCommandValidator(IGameRepository gameRepository)
    {
        RuleFor(x => x.FullGameId)
            .NotEmpty();

        RuleFor(x => x.Game.Name)
            .NotEmpty()
            .MaximumLength(100)
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var game = await gameRepository.GetByName(value, cancellationToken);

                if (game is not null)
                    context.AddFailure(DlcWithThisNameAlreadyExists);
            });


        RuleFor(x => x.Game.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Game.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Game.CurrencyId)
            .NotEmpty()
            .Must(x => new[] { 1, 2, 3 }.Contains(x))
            .WithMessage(CurrencyIdMustBe1Or2Or3);

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