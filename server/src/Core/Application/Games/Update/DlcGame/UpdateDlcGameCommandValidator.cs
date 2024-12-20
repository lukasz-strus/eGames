﻿using FluentValidation;

namespace Application.Games.Update.DlcGame;

internal sealed class UpdateDlcGameCommandValidator : AbstractValidator<UpdateDlcGameCommand>
{
    public UpdateDlcGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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