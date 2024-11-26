using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update.MakePublic;

public record UnpublishGameCommand(Guid Id) : IRequest<Result<Unit>>;