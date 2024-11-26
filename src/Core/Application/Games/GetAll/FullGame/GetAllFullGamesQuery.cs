using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.GetAll.FullGame;

public record GetAllFullGamesQuery(bool? IsPublished) : IRequest<Result<FullGameListResponse>>;