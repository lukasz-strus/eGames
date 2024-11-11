using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Update.Pay;

public record PayOrderCommand(Guid OrderId) : IRequest<Result<Unit>>;