using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update;

public record UpdateSubscriptionCommand(Guid Id, UpdateSubscriptionRequest Game) : IRequest<Result<Unit>>;