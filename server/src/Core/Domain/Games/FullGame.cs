﻿using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Games;

public sealed class FullGame : Game
{
    private readonly HashSet<DlcGame> _dlcGames = [];

    // ReSharper disable once UnusedMember.Local
    private FullGame()
    {
    }

    private FullGame(
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl) : base(name, description, price, releaseDate, publisher, downloadLink, fileSize, imageUrl)
    {
    }

    public IReadOnlyCollection<DlcGame> DlcGames => _dlcGames;

    public static FullGame Create(
        string name,
        string description,
        decimal price,
        Currency currency,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl,
        IEnumerable<DlcGame> dlcGames)
    {
        var game = new FullGame(
            name,
            description,
            new Money(currency, price),
            releaseDate,
            publisher,
            downloadLink,
            fileSize,
            imageUrl);

        game.InitializeDlcGames(dlcGames);

        return game;
    }

    private void InitializeDlcGames(IEnumerable<DlcGame> dlcGames)
    {
        _dlcGames.Clear();

        foreach (var dlcGame in dlcGames)
        {
            _dlcGames.Add(dlcGame);
        }
    }

    public override void Delete()
    {
        _dlcGames.ToList().ForEach(dlcGame => dlcGame.Delete());

        base.Delete();
    }

    public override void Restore()
    {
        _dlcGames.ToList().ForEach(dlcGame => dlcGame.Restore());

        base.Restore();
    }
}