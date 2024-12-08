using Domain.Games;
using FluentValidation;

namespace Application.Games.Create.Subscription;

internal sealed class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    private const string SubscriptionWithThisNameAlreadyExists = "Subscription with this name already exists.";

    public CreateSubscriptionCommandValidator(IGameRepository gameRepository)
    {
        RuleFor(x => x.Game.Name)
            .NotEmpty()
            .MaximumLength(100)
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var game = await gameRepository.GetByName(value, cancellationToken);

                if (game is not null)
                    context.AddFailure(SubscriptionWithThisNameAlreadyExists);
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

        RuleFor(x => x.Game.PeriodInDays)
            .NotEmpty();

        RuleFor(x => x.Game.ImageUrl)
            .NotEmpty();
    }
}