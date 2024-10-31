using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Create;

public record CreateOrderItemCommand(Guid OrderId, CreateOrderItemRequest OrderItem)
    : IRequest<Result<Unit>>;