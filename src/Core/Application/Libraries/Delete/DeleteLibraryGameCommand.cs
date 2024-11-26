using Domain.Core.Results;
using MediatR;

namespace Application.Libraries.Delete;

public record DeleteLibraryGameCommand(Guid LibraryGameId) : IRequest<Result<Unit>>;