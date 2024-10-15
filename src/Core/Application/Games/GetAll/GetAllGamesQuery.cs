using Application.Contracts.Games;
using MediatR;

namespace Application.Games.GetAll;

public record GetAllGamesQuery : IRequest<GameListResponse>;
