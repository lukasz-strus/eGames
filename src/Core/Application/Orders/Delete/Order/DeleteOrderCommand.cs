using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Delete.Order;

public record DeleteOrderCommand(Guid OrderId) : IRequest<Result<Unit>>;