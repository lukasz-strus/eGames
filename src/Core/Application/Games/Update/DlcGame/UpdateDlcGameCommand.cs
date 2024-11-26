using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update.DlcGame;

public record UpdateDlcGameCommand(Guid Id, UpdateGameRequest Game) : IRequest<Result<Unit>>;