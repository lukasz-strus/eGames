using Application.Contracts.Libraries;
using Domain.Core.Results;
using MediatR;

namespace Application.Libraries.Get;

public record GetLibraryGameByIdQuery(Guid LibraryGameId) : IRequest<Result<FullLibraryGameResponse>>;