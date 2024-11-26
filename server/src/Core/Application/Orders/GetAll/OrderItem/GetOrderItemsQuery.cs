using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.GetAll.OrderItem;

public record GetOrderItemsQuery(Guid OrderId) : IRequest<Result<OrderItemListResponse>>;