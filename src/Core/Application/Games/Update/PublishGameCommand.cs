using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update;

public record PublishGameCommand(Guid Id) : IRequest<Result<Unit>>;