using Domain.Core.Results;
using MediatR;

namespace Application.Users.Delete;

public record RemoveRoleCommand(Guid UserId, int RoleId) : IRequest<Result<Unit>>;