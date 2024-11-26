using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Get.DlcGame;

public record GetDlcGameByIdQuery(Guid Id) : IRequest<Result<DlcGameResponse>>;