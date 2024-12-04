using Application.Contracts.Common;
using Application.Contracts.Games;
using Application.Games.Create.DlcGame;
using Application.Games.Create.FullGame;
using Application.Games.Create.Subscription;
using Application.Games.Delete;
using Application.Games.Get.DlcGame;
using Application.Games.Get.FullGame;
using Application.Games.Get.Game;
using Application.Games.Get.Subscription;
using Application.Games.GetAll.DlcGame;
using Application.Games.GetAll.FullGame;
using Application.Games.GetAll.Game;
using Application.Games.GetAll.Subscription;
using Application.Games.Update.DlcGame;
using Application.Games.Update.FullGame;
using Application.Games.Update.MakePublic;
using Application.Games.Update.Restore;
using Application.Games.Update.Subscription;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Sieve.Models;

namespace Presentation.Controllers;

public class GameController(IMediator mediator) : ApiController(mediator)
{
    #region GetAll

    [HttpGet(ApiRoutes.Games.GetGames)]
    [ProducesResponseType(typeof(GameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGames(
        [FromQuery] SieveModel query,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllGamesQuery(query))
            .Bind(value => Mediator.Send(value, cancellationToken))
            .Match(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetFullGames)]
    [ProducesResponseType(typeof(FullGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFullGames(
        [FromQuery] SieveModel query,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllFullGamesQuery(query))
            .Bind(value => Mediator.Send(value, cancellationToken))
            .Match(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetDlcGames)]
    [ProducesResponseType(typeof(DlcGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDlcGames(
        Guid id,
        [FromQuery] SieveModel query,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllDlcGamesQuery(id, query))
            .Bind(value => Mediator.Send(value, cancellationToken))
            .Match(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetSubscriptions)]
    [ProducesResponseType(typeof(SubscriptionListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptions(
        [FromQuery] SieveModel query,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllSubscriptionsQuery(query))
            .Bind(value => Mediator.Send(value, cancellationToken))
            .Match(Ok, BadRequest);

    #endregion

    #region Get

    [HttpGet(ApiRoutes.Games.GetGame)]
    [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetFullGame)]
    [ProducesResponseType(typeof(FullGameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullGameById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetFullGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, NotFound);

    [HttpGet(ApiRoutes.Games.GetDlcGame)]
    [ProducesResponseType(typeof(DlcGameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDlcGameById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetDlcGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, NotFound);

    [HttpGet(ApiRoutes.Games.GetSubscription)]
    [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionGameById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetSubscriptionByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, NotFound);

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
            .Match(entityCreated => CreatedAtAction(
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
        Guid id,
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateDlcGameCommand(id, value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(entityCreated => CreatedAtAction(
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
            .Match(entityCreated => CreatedAtAction(
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
            .Match(NoContent, BadRequest);

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
            .Match(NoContent, BadRequest);

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
            .Match(NoContent, BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Games.PublishGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PublishGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new PublishGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(NoContent, BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Games.UnpublishGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UnpublishGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new UnpublishGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(NoContent, BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Games.RestoreGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RestoreGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new RestoreGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(NoContent, BadRequest);

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
            .Match(NoContent, BadRequest);

    #endregion
}