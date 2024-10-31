using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Update;

public record PayOrderCommand(Guid OrderId) : IRequest<Result<Unit>>;