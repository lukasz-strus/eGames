using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.GetAll;

public record GetUserOrdersQuery(Guid? UserId = null) : IRequest<Result<OrderListResponse>>;