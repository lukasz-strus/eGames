using Application.Contracts.Common;
using Application.Contracts.Games;
using Application.Games.Create;
using Application.Games.Delete;
using Application.Games.Get;
using Application.Games.GetAll;
using Application.Games.Update;
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
        [FromQuery] bool? isPublished,
        [FromQuery] bool? isSoftDeleted,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllGamesQuery(isPublished, isSoftDeleted))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<GameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetFullGames)]
    [ProducesResponseType(typeof(FullGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFullGames(
        [FromQuery] bool? isPublished,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllFullGamesQuery(isPublished))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<FullGameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetDlcGames)]
    [ProducesResponseType(typeof(DlcGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDlcGames(
        Guid fullGameId,
        [FromQuery] bool? isPublished,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllDlcGamesQuery(fullGameId, isPublished))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<DlcGameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetSubscriptions)]
    [ProducesResponseType(typeof(SubscriptionListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptions(
        [FromQuery] bool? isPublished,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllSubscriptionsQuery(isPublished))
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

    [Authorize(Roles = UserRoleNames.Admin)]
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
                entityCreated => CreatedAtAction(
                    nameof(GetFullGameById),
                    new { id = entityCreated.Id },
                    entityCreated),
                BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Games.CreateDlcGame)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateDlcGame(
        Guid fullGameId,
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateDlcGameCommand(fullGameId, value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<EntityCreatedResponse, IActionResult>(
                entityCreated => CreatedAtAction(
                    nameof(GetDlcGameById),
                    new { id = entityCreated.Id },
                    entityCreated),
                BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Games.CreateSubscription)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateSubscriptionCommand(value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<EntityCreatedResponse, IActionResult>(
                entityCreated => CreatedAtAction(
                    nameof(GetSubscriptionGameById),
                    new { id = entityCreated.Id },
                    entityCreated),
                BadRequest);

    #endregion

    #region Update

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPut(ApiRoutes.Games.UpdateFullGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateFullGame(
        Guid id,
        [FromBody] UpdateGameRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new UpdateFullGameCommand(id, value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPut(ApiRoutes.Games.UpdateDlcGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateDlcGame(
        Guid id,
        [FromBody] UpdateGameRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new UpdateDlcGameCommand(id, value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPut(ApiRoutes.Games.UpdateSubscription)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateSubscription(
        Guid id,
        [FromBody] UpdateSubscriptionRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new UpdateSubscriptionCommand(id, value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPatch(ApiRoutes.Games.PublishGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PublishGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new PublishGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPatch(ApiRoutes.Games.UnpublishGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UnpublishGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new UnpublishGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPatch(ApiRoutes.Games.RestoreGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RestoreGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new RestoreGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    #endregion

    #region Delete

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpDelete(ApiRoutes.Games.DeleteGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteGame(
        Guid id,
        [FromQuery] bool? destroy,
        CancellationToken cancellationToken) =>
        await Result.Success(new DeleteGameCommand(id, destroy))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    #endregion

    //TODO: PRZETESTOWAĆ !!! (Dodać testy w Postmanie)
    //TODO: Refactor excpetion middleware
    //TODO: Add logging
}