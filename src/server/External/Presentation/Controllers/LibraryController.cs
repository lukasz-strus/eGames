using Application.Contracts.Libraries;
using Application.Libraries.Delete;
using Application.Libraries.Get;
using Application.Libraries.GetAll;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public class LibraryController(IMediator mediator) : ApiController(mediator)
{
    #region GetAll

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpGet(ApiRoutes.Libraries.GetOwnLibraryGames)]
    [ProducesResponseType(typeof(LibraryGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnGames(CancellationToken cancellationToken) =>
        await Result.Success(new GetAllLibraryGamesQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, BadRequest);

    [Authorize(Roles = UserRoleNames.SuperAdmin)]
    [HttpGet(ApiRoutes.Libraries.GetUserLibraryGames)]
    [ProducesResponseType(typeof(LibraryGameListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserGames(
        Guid userId,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetAllLibraryGamesQuery(userId))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, BadRequest);

    #endregion

    #region Get

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpGet(ApiRoutes.Libraries.GetLibraryGame)]
    [ProducesResponseType(typeof(FullLibraryGameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLibraryGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetLibraryGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, NotFound);

    #endregion

    #region Delete

    [Authorize(Roles = UserRoleNames.SuperAdmin)]
    [HttpDelete(ApiRoutes.Libraries.DeleteLibraryGame)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteLibraryGame(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new DeleteLibraryGameCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(NoContent, BadRequest);

    #endregion
}