﻿using Application.Contracts.Orders;
using Domain.Core.Results;
using MediatR;

namespace Application.Orders.Get.Order;

public record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderResponse>>;