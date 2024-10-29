using Application.Contracts.Games;
using Application.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.GetAll;

internal sealed class GetAllSubscriptionsQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetAllSubscriptionsQuery, Result<SubscriptionListResponse>>
{
    public async Task<Result<SubscriptionListResponse>> Handle(
        GetAllSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var subscriptions = await gameRepository.GetAllSubscriptionsAsync(
            request.IsPublished,
            cancellationToken);

        var subscriptionListResponse = new SubscriptionListResponse(
            [
                ..subscriptions.Select(g => g.ToResponse())
            ]
        );
        return Result.Success(subscriptionListResponse);
    }
}