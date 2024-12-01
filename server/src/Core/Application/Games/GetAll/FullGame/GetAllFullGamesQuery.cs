using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;
using Sieve.Models;

namespace Application.Games.GetAll.FullGame;

public record GetAllFullGamesQuery(SieveModel Query) : IRequest<Result<FullGameListResponse>>;