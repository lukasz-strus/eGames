using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update;

public record UnpublishGameCommand(Guid Id) : IRequest<Result<Unit>>;