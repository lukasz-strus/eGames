using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.GetAll;

public record GetAllDlcGamesQuery(Guid FullGameId, bool? IsPublished) : IRequest<Result<DlcGameListResponse>>;