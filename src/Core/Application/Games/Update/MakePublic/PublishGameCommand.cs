using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update.MakePublic;

public record PublishGameCommand(Guid Id) : IRequest<Result<Unit>>;