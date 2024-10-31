using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Get;

public record GetOrderItemByIdQuery(Guid OrderItemId) : IRequest<Result<OrderItemResponse>>;