using Application.Contracts.Games;
using MediatR;

namespace Application.Games.Get;

public record GetGameByIdQuery(Guid Id) : IRequest<GameResponse>;