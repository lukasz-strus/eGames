﻿using Application.Contracts.Common;
using Application.Contracts.Orders;
using Application.Orders.Create.Order;
using Application.Orders.Create.OrderItem;
using Application.Orders.Delete.Order;
using Application.Orders.Delete.OrderItem;
using Application.Orders.Get.Order;
using Application.Orders.Get.OrderItem;
using Application.Orders.GetAll;
using Application.Orders.GetAll.OrderItem;
using Application.Orders.Update.Pay;
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
    #region GetAll

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpGet(ApiRoutes.Orders.Users.GetUserOrders)]
    [ProducesResponseType(typeof(OrderListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserOrders(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(new GetUserOrdersQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, BadRequest);


    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpGet(ApiRoutes.Orders.Users.GetOwnOrders)]
    [ProducesResponseType(typeof(OrderListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetOwnOrders(
        CancellationToken cancellationToken) =>
        await Result.Success(new GetUserOrdersQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match(Ok, BadRequest);

    #endregion

    #region Get

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
            .Match(Ok, NotFound);

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
            .Match(Ok, NotFound);

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
            .Match(Ok, NotFound);

    #endregion

    #region Create

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
            .Match(entityCreated => CreatedAtAction(
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
            .Match(NoContent, NotFound);

    #endregion

    #region Update

    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpPost(ApiRoutes.Orders.PayOrder)]
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
            .Match(NoContent, NotFound);

    #endregion

    #region Delete

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
            .Match(NoContent, NotFound);


    [Authorize(Roles = UserRoleNames.Customer)]
    [HttpDelete(ApiRoutes.Orders.DeleteOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(
        Guid id,
        CancellationToken cancellationToken) =>
        await Result.Success(id)
            .Map(value => new DeleteOrderCommand(value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(NoContent, NotFound);

    #endregion
}