using Domain.Core.Results;
using MediatR;

namespace Application.Users.Update;

public record UnbanUserCommand(Guid UserId) : IRequest<Result<Unit>>;