﻿namespace Domain.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
}
