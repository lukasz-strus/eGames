using Application.Contracts.Games;
using Application.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get.FullGame;

internal sealed class GetFullGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetFullGameByIdQuery, Result<FullGameResponse>>
{
    public async Task<Result<FullGameResponse>> Handle(GetFullGameByIdQuery request,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetFullGameByIdAsync(new GameId(request.Id), cancellationToken);

        return game is not null
            ? Result.Success(game.ToResponse())
            : Result.Failure<FullGameResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));
    }
}