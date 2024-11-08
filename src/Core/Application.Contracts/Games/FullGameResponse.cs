﻿namespace Application.Contracts.Games;

public sealed class FullGameResponse(
    Guid id,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    string downloadLink,
    ulong fileSize,
    IEnumerable<DlcGameResponse> dlcGame)
    : GameResponse(
        id,
        "Full game",
        name,
        description,
        currency,
        amount,
        releaseDate,
        publisher,
        downloadLink,
        fileSize)
{
    public IEnumerable<DlcGameResponse> DlcGames { get; } = dlcGame;
}