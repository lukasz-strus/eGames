using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Create.OrderItem;

public record CreateOrderItemCommand(Guid OrderId, CreateOrderItemRequest OrderItem)
    : IRequest<Result<Unit>>;