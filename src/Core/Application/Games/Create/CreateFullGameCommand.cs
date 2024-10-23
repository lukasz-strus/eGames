using Application.Contracts.Common;
using Application.Contracts.Games;
using MediatR;

namespace Application.Games.Create;

public record CreateFullGameCommand(CreateGameRequest Game) : IRequest<EntityCreatedResponse>;