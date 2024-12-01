using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;
using Sieve.Models;

namespace Application.Games.GetAll.DlcGame;

public record GetAllDlcGamesQuery(Guid FullGameId, SieveModel Query) : IRequest<Result<DlcGameListResponse>>;