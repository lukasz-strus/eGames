using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;
using Sieve.Models;

namespace Application.Games.GetAll.Game;

public record GetAllGamesQuery(SieveModel Query) : IRequest<Result<GameListResponse>>;