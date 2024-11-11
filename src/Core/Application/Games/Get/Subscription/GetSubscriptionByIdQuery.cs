using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Get.Subscription;

public record GetSubscriptionByIdQuery(Guid Id) : IRequest<Result<SubscriptionResponse>>;