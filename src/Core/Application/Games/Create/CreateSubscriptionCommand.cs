using Application.Contracts.Common;
using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Create;

public record CreateSubscriptionCommand(CreateSubscriptionRequest Game) : IRequest<Result<EntityCreatedResponse>>;