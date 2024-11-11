using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Delete.OrderItem;

public record DeleteOrderItemCommand(Guid OrderId, Guid OrderItemId) : IRequest<Result<Unit>>;