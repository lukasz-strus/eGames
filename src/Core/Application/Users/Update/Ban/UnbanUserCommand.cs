using Domain.Core.Results;
using MediatR;

namespace Application.Users.Update.Ban;

public record UnbanUserCommand(Guid UserId) : IRequest<Result<Unit>>;