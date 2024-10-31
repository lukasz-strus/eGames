using Application.Contracts.Common;
using Application.Contracts.Orders;
using Application.Orders.Create;
using Application.Orders.Delete;
using Application.Orders.Get;
using Application.Orders.GetAll;
using Application.Orders.Update;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public class OrderController(IMediator mediator) : ApiController(mediator)
{
    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpGet(ApiRoutes.Orders.GetById)]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(id)
            .Bind(value => Mediator.Send(new GetOrderByIdQuery(value), cancellationToken))
            .Match<OrderResponse, IActionResult>(Ok, NotFound);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpGet(ApiRoutes.Orders.GetOrderItems)]
    [ProducesResponseType(typeof(OrderItemListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderItems(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(id)
            .Bind(value => Mediator.Send(new GetOrderItemsQuery(value), cancellationToken))
            .Match<OrderItemListResponse, IActionResult>(Ok, NotFound);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpGet(ApiRoutes.Orders.GetOrderItem)]
    [ProducesResponseType(typeof(OrderItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderItemById(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(id)
            .Bind(value => Mediator.Send(new GetOrderItemByIdQuery(value), cancellationToken))
            .Match<OrderItemResponse, IActionResult>(Ok, NotFound);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpPost(ApiRoutes.Orders.CreateOrder)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateOrder(
        CancellationToken cancellationToken) =>
        await Result.Success(User.Claims)
            .Map(_ => new CreateOrderCommand())
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<EntityCreatedResponse, IActionResult>(
                entityCreated => CreatedAtAction(
                    nameof(GetOrderById),
                    new { id = entityCreated.Id },
                    entityCreated),
                BadRequest);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpPost(ApiRoutes.Orders.CreateOrderItem)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddOrderItem(
        Guid id,
        CreateOrderItemRequest request,
        CancellationToken cancellationToken) =>
        await Result.Success((id, request))
            .Map(value => new CreateOrderItemCommand(value.id, value.request))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(_ => NoContent(), NotFound);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpDelete(ApiRoutes.Orders.RemoveOrderItem)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveOrderItem(
        Guid id,
        Guid itemId,
        CancellationToken cancellationToken) =>
        await Result.Success((id, itemId))
            .Map(value => new DeleteOrderItemCommand(value.id, value.itemId))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(_ => NoContent(), NotFound);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpPatch(ApiRoutes.Orders.PayOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PayOrder(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(id)
            .Map(value => new PayOrderCommand(value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(_ => NoContent(), NotFound);

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpPatch(ApiRoutes.Orders.CancelOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelOrder(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(id)
            .Map(value => new CancelOrderCommand(value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<Unit, IActionResult>(_ => NoContent(), NotFound);
}