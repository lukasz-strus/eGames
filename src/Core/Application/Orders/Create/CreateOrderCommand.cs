using Application.Contracts.Common;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Create;

public record CreateOrderCommand : IRequest<Result<EntityCreatedResponse>>;