using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update.Subscription;

public record UpdateSubscriptionCommand(Guid Id, UpdateSubscriptionRequest Game) : IRequest<Result<Unit>>;