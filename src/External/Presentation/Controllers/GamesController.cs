﻿using Application.Contracts.Common;
using Application.Contracts.Games;
using Application.Games.Create;
using Application.Games.Get;
using Application.Games.GetAll;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public class GamesController(IMediator mediator) : ApiController(mediator)
{
    #region GetAll

    [HttpGet(ApiRoutes.Games.GetGames)]
    [ProducesResponseType(typeof(GameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGames(
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllGamesQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<GameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetFullGames)]
    [ProducesResponseType(typeof(FullGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFullGames(
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllFullGamesQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<FullGameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetDlcGames)]
    [ProducesResponseType(typeof(DlcGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDlcGames(
        Guid fullGameId,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllDlcGamesQuery(fullGameId))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<DlcGameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetSubscriptions)]
    [ProducesResponseType(typeof(SubscriptionListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptions(
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllSubscriptionsQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<SubscriptionListResponse, IActionResult>(Ok, BadRequest);

    #endregion

    #region Get

    [HttpGet(ApiRoutes.Games.GetFullGame)]
    [ProducesResponseType(typeof(FullGameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullGameById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetFullGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<FullGameResponse, IActionResult>(Ok, NotFound);

    [HttpGet(ApiRoutes.Games.GetDlcGame)]
    [ProducesResponseType(typeof(DlcGameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDlcGameById(
        Guid fullGameId,
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetDlcGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<DlcGameResponse, IActionResult>(Ok, NotFound);

    [HttpGet(ApiRoutes.Games.GetSubscription)]
    [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionGameById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetSubscriptionByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<SubscriptionResponse, IActionResult>(Ok, NotFound);

    #endregion

    #region Create

    [Authorize(UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Games.CreateFullGame)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateFullGame(
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateFullGameCommand(value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<EntityCreatedResponse, IActionResult>(
                entityCreated => CreatedAtAction(nameof(GetFullGameById), new { id = entityCreated.Id }, entityCreated),
                BadRequest);

    #endregion

    //TODO: Dodać endpointy do tworzenia, edytowania i usuwania gier (rózne dla różnych typów)
    //TODO: Refactor excpetion middleware
    //TODO: Add logging
}