using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update.Restore;

public record RestoreGameCommand(Guid Id) : IRequest<Result<Unit>>;