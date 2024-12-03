using Application.Contracts.Games;
using Application.Core.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Application.Games.GetAll.Subscription;

internal sealed class GetAllSubscriptionsQueryHandler(
    IGameRepository gameRepository,
    ISieveProcessor sieveProcessor) : IRequestHandler<GetAllSubscriptionsQuery, Result<SubscriptionListResponse>>
{
    public async Task<Result<SubscriptionListResponse>> Handle(
        GetAllSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var subscriptionsQuery = gameRepository.GetAllSubscriptions();

        var subscriptions = await sieveProcessor
            .Apply(request.Query, subscriptionsQuery)
            .ToListAsync(cancellationToken);

        var totalCount = await sieveProcessor
            .Apply(request.Query, subscriptionsQuery, applyPagination: false)
            .CountAsync(cancellationToken);

        var subscriptionListResponse = new SubscriptionListResponse(
            [
                ..subscriptions.Select(g => g.ToResponse())
            ],
            totalCount,
            request.Query.PageSize,
            request.Query.Page);

        return Result.Success(subscriptionListResponse);
    }
}