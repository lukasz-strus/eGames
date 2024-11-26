using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Get.OrderItem;

public record GetOrderItemByIdQuery(Guid OrderItemId) : IRequest<Result<OrderItemResponse>>;