﻿using Application.Contracts.Games;
using Domain.Games;

namespace Application.Mappers;

internal static class GameMapper
{
    internal static GameResponse ToResponse(this Game game) =>
        new(game.Id.Value,
            game.GetType().Name,
            game.Name,
            game.Description,
            game.Price.Currency.Code,
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher,
            game.DownloadLink,
            game.FileSize);

    internal static FullGameResponse ToResponse(this FullGame game) =>
        new(game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.Code,
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher,
            game.DownloadLink,
            game.FileSize,
            game.DlcGames.Select(x => x.ToResponse()));

    internal static DlcGameResponse ToResponse(this DlcGame game) =>
        new(game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.Code,
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher,
            game.DownloadLink,
            game.FileSize,
            game.FullGameId.Value);

    internal static SubscriptionResponse ToResponse(this Subscription game) =>
        new(game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.Code,
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher,
            game.DownloadLink,
            game.FileSize,
            game.PeriodInDays);
}