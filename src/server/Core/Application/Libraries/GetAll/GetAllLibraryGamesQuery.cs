using Application.Contracts.Libraries;
using Domain.Core.Results;
using MediatR;

namespace Application.Libraries.GetAll;

public record GetAllLibraryGamesQuery(Guid? UserId = null) : IRequest<Result<LibraryGameListResponse>>;