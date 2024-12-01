using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;
using Sieve.Models;

namespace Application.Games.GetAll.Subscription;

public record GetAllSubscriptionsQuery(SieveModel Query) : IRequest<Result<SubscriptionListResponse>>;