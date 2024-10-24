using Application.Contracts.Common;
using Domain.Core.Results;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Create;

public record CreateOrderCommand(Guid CustomerId) : IRequest<Result<EntityCreatedResponse>>;