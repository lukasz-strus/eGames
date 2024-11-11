using Domain.Core.Results;
using MediatR;

namespace Application.Users.Update.Role;

public record AddRoleCommand(Guid UserId, int RoleId) : IRequest<Result<Unit>>;