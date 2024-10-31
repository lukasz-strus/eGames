using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Update;

public record CancelOrderCommand(Guid OrderId) : IRequest<Result<Unit>>;