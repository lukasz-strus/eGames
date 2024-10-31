using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Delete;

public record DeleteOrderItemCommand(Guid OrderId, Guid OrderItemId) : IRequest<Result<Unit>>;