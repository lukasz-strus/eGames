using Application.Contracts.User;
using Application.Users.Delete;
using Application.Users.Get;
using Application.Users.GetAll;
using Application.Users.Update;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

[Authorize(Roles = UserRoleNames.SuperAdmin)]
public class UserController(IMediator mediator) : ApiController(mediator)
{
    #region GetAll

    [HttpGet(ApiRoutes.Users.GetAll)]
    [ProducesResponseType(typeof(UserListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        await Result.Success(new GetAllUsersQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<UserListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Users.GetAllRoles)]
    [ProducesResponseType(typeof(UserRoleListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken) =>
        await Result.Success(new GetAllRolesQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<UserRoleListResponse, IActionResult>(Ok, BadRequest);

    #endregion

    #region Get

    [HttpGet(ApiRoutes.Users.Get)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetUserQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<UserResponse, IActionResult>(Ok, NotFound);

    [HttpGet(ApiRoutes.Users.GetUserRoles)]
    [ProducesResponseType(typeof(UserRoleListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserRoles(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetUserRolesQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<UserRoleListResponse, IActionResult>(Ok, NotFound);

    #endregion

    #region Delete

    [HttpDelete(ApiRoutes.Users.RemoveRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveRole(
        [FromRoute] Guid id,
        [FromRoute] int roleId,
        CancellationToken cancellationToken) =>
        await Result.Success(new RemoveRoleCommand(id, roleId))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    #endregion

    #region Update

    [HttpPost(ApiRoutes.Users.AddRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddRole(
        [FromRoute] Guid id,
        [FromBody] AddRoleToUserRequest request,
        CancellationToken cancellationToken) =>
        await Result.Success(new AddRoleCommand(id, request.RoleId))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    [HttpPost(ApiRoutes.Users.Ban)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Ban(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new BanUserCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());


    [HttpPost(ApiRoutes.Users.Unban)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Unban(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new UnbanUserCommand(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(
                _ => NoContent(),
                _ => BadRequest());

    #endregion
}