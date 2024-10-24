using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Get;

public record GetGameByIdQuery(Guid Id) : IRequest<Result<GameResponse>>;