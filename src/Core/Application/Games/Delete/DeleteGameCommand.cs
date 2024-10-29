using Domain.Core.Results;
using MediatR;

namespace Application.Games.Delete;

public record DeleteGameCommand(Guid Id, bool? Destroy) : IRequest<Result<Unit>>;