using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Delete;

public record DeleteOrderCommand(Guid OrderId) : IRequest<Result<Unit>>;