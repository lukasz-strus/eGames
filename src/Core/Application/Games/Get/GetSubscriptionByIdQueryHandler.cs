using Application.Contracts.Games;
using Application.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get;

internal sealed class GetSubscriptionByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionResponse>>
{
    public async Task<Result<SubscriptionResponse>> Handle(
        GetSubscriptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetSubscriptionByIdAsync(new GameId(request.Id), cancellationToken);

        return game is not null
            ? Result.Success(game.ToResponse())
            : Result.Failure<SubscriptionResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));
    }
}