using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.GetAll.Subscription;

public record GetAllSubscriptionsQuery(bool? IsPublished) : IRequest<Result<SubscriptionListResponse>>;